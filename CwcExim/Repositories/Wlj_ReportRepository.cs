using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using CwcExim.Areas.Report.Models;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
using Newtonsoft.Json;
using CwcExim.Areas.Import.Models;
using System.Dynamic;
using CwcExim.Areas.CashManagement.Models;

namespace CwcExim.Repositories   
{
    public class Wlj_ReportRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }       

        public void GetInvoiceDetailsForPrint(int InvoiceId, string InvoiceType = "GE")
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceType });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBulkInvoiceDetailsForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PpgInvoiceGate objPaymentSheet = new PpgInvoiceGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheet.CompanyName = Result["CompanyName"].ToString();
                    objPaymentSheet.CompanyShortName = Result["CompanyShortName"].ToString();
                    objPaymentSheet.CompanyAddress = Result["CompanyAddress"].ToString();
                    objPaymentSheet.CompanyGstNo = Result["GstIn"].ToString();
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPaymentSheet.InvoiceNo = Result["InvoiceNo"].ToString();
                        objPaymentSheet.InvoiceDate = Result["InvoiceDate"].ToString();
                        objPaymentSheet.PartyName = Result["PartyName"].ToString();
                        objPaymentSheet.PartyState = Result["PartyState"].ToString();
                        objPaymentSheet.PartyAddress = Result["PartyAddress"].ToString();
                        objPaymentSheet.PartyStateCode = Result["PartyStateCode"].ToString();
                        objPaymentSheet.PartyGstNo = Result["PartyGSTNo"].ToString();
                        objPaymentSheet.TotalTax = Convert.ToDecimal(Result["TotalTaxable"]);
                        objPaymentSheet.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                        objPaymentSheet.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                        objPaymentSheet.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                        objPaymentSheet.TotalAmt = Convert.ToDecimal(Result["InvoiceAmt"]);

                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPaymentSheet.LstContainersGate.Add(new PpgInvoiceContainerGate()
                        {
                            CfsCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            FromDate = Convert.ToString(Result["FromDate"]),
                            ToDate = Convert.ToString(Result["ToDate"])
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPaymentSheet.LstChargesGate.Add(new PpgInvoiceChargeGate()
                        {
                            ChargeSD = Convert.ToString(Result["OperationSDesc"]),
                            ChargeDesc = Convert.ToString(Result["OperationDesc"]),
                            HsnCode = Convert.ToString(Result["SACCode"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            TaxableAmt = Convert.ToDecimal(Result["Taxable"]),

                            CGSTRate = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTRate = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            IGSTRate = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),

                        });
                    }
                }

                //-------------------------------------------------------------------------
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheet;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
                //-----------------------------------------------------------------------
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

        public List<PpgInvoiceGate> GetBulkInvoiceDetailsForPrint(BulkInvoiceReport ObjBulkInvoiceReport)
        {
            DateTime dtfrom = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            if (String.IsNullOrWhiteSpace(ObjBulkInvoiceReport.InvoiceModule))
            {
                ObjBulkInvoiceReport.InvoiceModule = "";
            }
            if (String.IsNullOrWhiteSpace(ObjBulkInvoiceReport.InvoiceNumber))
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }

            List<PpgInvoiceGate> objPaymentSheetList = new List<PpgInvoiceGate>();
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceModule });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceNumber });

            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBulkInvoiceDetailsForPrint", CommandType.StoredProcedure, DParam);
                IList<string> htmls = new List<string>();
                _DBResponse = new DatabaseResponse();




                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    PpgInvoiceGate objPaymentSheetInvoiceHeader = new PpgInvoiceGate();
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        objPaymentSheetInvoiceHeader.CompanyName = dr["CompanyName"].ToString();
                        objPaymentSheetInvoiceHeader.CompanyShortName = dr["CompanyShortName"].ToString();
                        objPaymentSheetInvoiceHeader.CompanyAddress = dr["CompanyAddress"].ToString();
                        objPaymentSheetInvoiceHeader.CompanyGstNo = dr["GstIn"].ToString();
                    }

                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        PpgInvoiceGate objPaymentSheet = new PpgInvoiceGate();
                        objPaymentSheet.CompanyName = objPaymentSheetInvoiceHeader.CompanyName;
                        objPaymentSheet.CompanyShortName = objPaymentSheetInvoiceHeader.CompanyShortName;
                        objPaymentSheet.CompanyAddress = objPaymentSheetInvoiceHeader.CompanyAddress;
                        objPaymentSheet.CompanyGstNo = objPaymentSheetInvoiceHeader.CompanyGstNo;

                        objPaymentSheet.InvoiceId = Convert.ToInt32(dr["InvoiceId"]);
                        objPaymentSheet.InvoiceNo = dr["InvoiceNo"].ToString();
                        objPaymentSheet.InvoiceDate = dr["InvoiceDate"].ToString();
                        objPaymentSheet.PartyName = dr["PartyName"].ToString();
                        objPaymentSheet.PartyState = dr["PartyState"].ToString();
                        objPaymentSheet.PartyAddress = dr["PartyAddress"].ToString();
                        objPaymentSheet.PartyStateCode = dr["PartyStateCode"].ToString();
                        objPaymentSheet.PartyGstNo = dr["PartyGSTNo"].ToString();
                        objPaymentSheet.TotalTax = Convert.ToDecimal(dr["TotalTaxable"]);
                        objPaymentSheet.TotalCGST = Convert.ToDecimal(dr["TotalCGST"]);
                        objPaymentSheet.TotalSGST = Convert.ToDecimal(dr["TotalSGST"]);
                        objPaymentSheet.TotalIGST = Convert.ToDecimal(dr["TotalIGST"]);
                        objPaymentSheet.TotalAmt = Convert.ToDecimal(dr["InvoiceAmt"]);

                        foreach (DataRow drContainer in Result.Tables[2].Rows)
                        {
                            if (objPaymentSheet.InvoiceId == Convert.ToInt32(drContainer["InoviceId"]))
                            {
                                objPaymentSheet.LstContainersGate.Add(new PpgInvoiceContainerGate()
                                {
                                    CfsCode = Convert.ToString(drContainer["CFSCode"]),
                                    ContainerNo = Convert.ToString(drContainer["ContainerNo"]),
                                    Size = Convert.ToString(drContainer["Size"]),
                                    FromDate = Convert.ToString(drContainer["FromDate"]),
                                    ToDate = Convert.ToString(drContainer["ToDate"])
                                });
                            }
                        }

                        foreach (DataRow drCharges in Result.Tables[3].Rows)
                        {
                            if (objPaymentSheet.InvoiceId == Convert.ToInt32(drCharges["InoviceId"]))
                            {
                                objPaymentSheet.LstChargesGate.Add(new PpgInvoiceChargeGate()
                                {
                                    ChargeSD = Convert.ToString(drCharges["OperationSDesc"]),
                                    ChargeDesc = Convert.ToString(drCharges["OperationDesc"]),
                                    HsnCode = Convert.ToString(drCharges["SACCode"]),
                                    Rate = Convert.ToDecimal(drCharges["Rate"]),
                                    TaxableAmt = Convert.ToDecimal(drCharges["Taxable"]),

                                    CGSTRate = Convert.ToDecimal(drCharges["CGSTPer"]),
                                    CGSTAmt = Convert.ToDecimal(drCharges["CGSTAmt"]),
                                    SGSTRate = Convert.ToDecimal(drCharges["SGSTPer"]),
                                    SGSTAmt = Convert.ToDecimal(drCharges["SGSTAmt"]),
                                    IGSTRate = Convert.ToDecimal(drCharges["IGSTPer"]),
                                    IGSTAmt = Convert.ToDecimal(drCharges["IGSTAmt"]),
                                    Total = Convert.ToDecimal(drCharges["Total"]),

                                });
                            }
                        }

                        objPaymentSheetList.Add(objPaymentSheet);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = null;
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

            return objPaymentSheetList;
        }

        public void GenericBulkInvoiceDetailsForPrint(BulkInvoiceReport ObjBulkInvoiceReport)
        {
            DateTime dtfrom = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            if (String.IsNullOrWhiteSpace(ObjBulkInvoiceReport.InvoiceModule))
            {
                ObjBulkInvoiceReport.InvoiceModule = "";
            }
            if (String.IsNullOrWhiteSpace(ObjBulkInvoiceReport.InvoiceNumber))
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceModule });

            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBulkInvoiceDetailsForPrint", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[1].Rows.Count > 0)
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

        public void GetInvoiceList(string FromDate, string ToDate, string invoiceType)
        {

            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");


            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.String, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.String, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = invoiceType });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("InvoiceListWithDate", CommandType.StoredProcedure, DParam);
            IList<invoiceLIst> LstInvoice = new List<invoiceLIst>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstInvoice.Add(new invoiceLIst
                    {



                        InvoiceNumber = Result["InvoiceNumber"].ToString()



                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = JsonConvert.SerializeObject(LstInvoice); ;
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

        public void ModuleListWithInvoice(BulkInvoiceReport ObjBulkInvoiceReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            int PartyId = ObjBulkInvoiceReport.PartyId;

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.String, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.String, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            DParam = LstParam.ToArray();
            DataSet LstInvoice = new DataSet();
            LstInvoice = DataAccess.ExecuteDataSet("ModuleListWithInvoice", CommandType.StoredProcedure, DParam);
            
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                if (LstInvoice != null)
                {
                    Status = 1;
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInvoice;
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
                //Result.Close();
                //Result.Dispose();

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
                    objCompanyDetails.EffectVersion = Convert.ToDecimal(Result["Version"]);
                    objCompanyDetails.VersionLogoFile = Convert.ToString(Result["Effectlogofile"]);


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

        public void GetSDBalanceforReport(int PayeeId, int invid)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_invoiceId", MySqlDbType = MySqlDbType.Int32, Value = invid });

            //    lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("getSDBalanceForprint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
            try
            {


                while (Result.Read())
                {
                    Status = 1;
                    objSDBalance.SDBalance = Convert.ToDecimal(Result["SDBalanceAmount"]);

                }


                if (Status == 1)
                {
                    _DBResponse.Data = objSDBalance;
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




        public string GetPreviousInvDate(String DelAppNo)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_DeliApp", MySqlDbType = MySqlDbType.VarChar, Value = DelAppNo });
       
            //    lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetPreviousInvoiceDate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
           // PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
            try
            {


                while (Result.Read())
                {
                    Status = 1;
                     ReturnObj =  Result["PDate"].ToString();

                }


                if (Status == 1)
                {
                    _DBResponse.Data = ReturnObj;
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
            return ReturnObj;
        }


        public void GetDaysWeeksForIMPYard(int invid,string CFSCode,int fvalue)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();            
            lstParam.Add(new MySqlParameter { ParameterName = "in_invoiceId", MySqlDbType = MySqlDbType.Int32, Value = invid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_flag", MySqlDbType = MySqlDbType.Int32, Value = fvalue });

            //    lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetDaysWeeksForIMPYard", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            PpgDaysWeeks objDaysWeeks = new PpgDaysWeeks();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDaysWeeks.Days = Convert.ToInt32(Result["Days"]);
                    //objDaysWeeks.Weeks = Convert.ToInt32(Result["Weeks"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objDaysWeeks;
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
        public void GetPartyList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyList", CommandType.StoredProcedure, DParam);
            List<dynamic> lstParty = new List<dynamic>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new { PartyId = Convert.ToInt32(Result["PartyId"]), PartyName = Convert.ToString(Result["PartyName"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstParty;
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

        #region  SD A/c Statement
        public void GetPDAStatement(int Month, int Year)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.Int32, Value = Month });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = Year });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("RptSdStatement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            SDStatement ObjSDStatement = new SDStatement();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        ObjSDStatement.LstSD.Add(new PPGSDList
                        {
                            PartyName = Convert.ToString(dr["PartyName"]),
                            SDAmount = Convert.ToString(dr["SDAmount"]),
                            UnpaidAmount = Convert.ToString(dr["UnpaidAmount"]),
                            BalanceAmount = Convert.ToString(dr["BalanceAmount"]),
                            AdjustAmount = Convert.ToDecimal(dr["AdjustAmount"]),
                            UtilizationAmount = Convert.ToDecimal(dr["UtilizationAmount"]),
                             RefundAmount = Convert.ToDecimal(dr["RefundAmount"])


                            
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSDStatement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
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

        #region Register of Outward Supply
        public void GetRegisterofOutwardSupply(DateTime date1, DateTime date2,string Type)
        {
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetRegisterofOutwardSupply", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            if(Type== "Inv" || Type == "Unpaid")
            {
                List<PpgRegisterOfOutwardSupplyModel> model = new List<PpgRegisterOfOutwardSupplyModel>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        model = (from DataRow dr in dt.Rows
                                 select new PpgRegisterOfOutwardSupplyModel()
                                 {
                                     SlNo = Convert.ToInt32(dr["SlNo"]),
                                     GST = dr["GST"].ToString(),
                                     Place = dr["Place"].ToString(),
                                     Name = dr["Name"].ToString(),
                                     Period = dr["Period"].ToString(),

                                     Nature = dr["Nature"].ToString(),
                                     Rate = Convert.ToDecimal(dr["Rate"]),
                                     InvoiceNo = dr["InvoiceNo"].ToString(),
                                     InvoiceDate = dr["InvoiceDate"].ToString(),
                                     ServiceValue = Convert.ToDecimal(dr["ServiceValue"]),

                                     ITaxPercent = dr["ITaxPercent"].ToString(),
                                     ITaxAmount = Convert.ToDecimal(dr["ITaxAmount"]),
                                     CTaxPercent = dr["CTaxPercent"].ToString(),
                                     CTaxAmount = Convert.ToDecimal(dr["CTaxAmount"]),
                                     STaxPercent = dr["STaxPercent"].ToString(),
                                     STaxAmount = Convert.ToDecimal(dr["STaxAmount"]),

                                     Total = Convert.ToDecimal(dr["Total"]),
                                     //WH = dr["WH"].ToString(),
                                     //CRNoDate = dr["CRNoDate"].ToString(),
                                     //SD = Convert.ToDecimal(dr["SD"]),
                                     //Amount = Convert.ToDecimal(dr["Amount"]),
                                     //TDS = Convert.ToDecimal(dr["TDS"]),
                                     //ChequeNoDate = dr["CHDD"].ToString(),
                                     PaymentMode = dr["PaymentMode"].ToString(),
                                     Remarks = dr["Remarks"].ToString()
                                     ,HSNCode= dr["HSNCode"].ToString()
                                 }).ToList();
                    }
                    decimal InvoiceAmount = 0, CRAmount = 0;
                    //foreach (DataRow dr in ds.Tables[1].Rows)
                    //{
                    //    InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                    //    CRAmount = Convert.ToDecimal(dr["CRAmount"]);
                    //}
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = RegisterofOutwardSupplyExcel(model, InvoiceAmount, CRAmount);
                }
                catch (Exception ex)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
                finally
                {
                    ds.Dispose();
                }
            }
            else
            {
                List<PpgRegisterOfOutwardSupplyModelCreditDebit> modelCreditDebit = new List<PpgRegisterOfOutwardSupplyModelCreditDebit>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        modelCreditDebit = (from DataRow dr in dt.Rows
                                 select new PpgRegisterOfOutwardSupplyModelCreditDebit()
                                 {
                                     SlNo = Convert.ToInt32(dr["SlNo"]),
                                     GST = dr["GST"].ToString(),
                                     Place = dr["Place"].ToString(),
                                     Name = dr["Name"].ToString(),
                                     Period = dr["Period"].ToString(),

                                     Nature = dr["Nature"].ToString(),
                                     HSNCode = dr["HSNCode"].ToString(),
                                     Rate = Convert.ToDecimal(dr["Rate"]),
                                     CreditNoteNo=dr["CreditNoteNo"].ToString(),
                                     CRNoteDate=dr["CRNoteDate"].ToString(),
                                     InvoiceNo = dr["InvoiceNo"].ToString(),
                                     InvoiceDate = dr["InvoiceDate"].ToString(),
                                     ServiceValue = Convert.ToDecimal(dr["ServiceValue"]),

                                     ITaxPercent = dr["ITaxPercent"].ToString(),
                                     ITaxAmount = Convert.ToDecimal(dr["ITaxAmount"]),
                                     CTaxPercent = dr["CTaxPercent"].ToString(),
                                     CTaxAmount = Convert.ToDecimal(dr["CTaxAmount"]),
                                     STaxPercent = dr["STaxPercent"].ToString(),
                                     STaxAmount = Convert.ToDecimal(dr["STaxAmount"]),

                                     Total = Convert.ToDecimal(dr["Total"]),                                                                         
                                     Remarks = dr["Remarks"].ToString()
                                     
                                 }).ToList();
                    }
                    decimal InvoiceAmount = 0, CRAmount = 0;
                    //foreach (DataRow dr in ds.Tables[1].Rows)
                    //{
                    //    InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                    //    CRAmount = Convert.ToDecimal(dr["CRAmount"]);
                    //}
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = RegisterofOutwardSupplyExcelCreditDebit(modelCreditDebit, InvoiceAmount, CRAmount);
                }
                catch (Exception ex)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
                finally
                {
                    ds.Dispose();
                }
            }

            
        }
        private string RegisterofOutwardSupplyExcel(List<PpgRegisterOfOutwardSupplyModel> model, decimal InvoiceAmount, decimal CRAmount)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "Principal Place of Business"
                        + Environment.NewLine + "CENTRAL WAREHOUSE"
                        + Environment.NewLine + Environment.NewLine
                        + "REGISTER OF OUTWARD SUPPLY (TAX INVOICE/BILL OF SUPPLY)";
                exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N1:O1", "BILL REGISTER", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:A4", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B2:B4", "GSTIN", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C2:C4", "Place" + Environment.NewLine + "(Name of State)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D2:D4", "Name of Customer to whom Service rendered", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E2:E4", "Period of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F2:F4", "Nature of Invoice" + Environment.NewLine + "(Resv./Initial Fumigatiom/General Basic/Over & Above)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G2:G4", "HSN Code", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H2:H4", "Rate per" + Environment.NewLine + "Bag/MT/Sqm", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I2:K2", "Invoice Details", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I3:I4", "Invoice No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J3:J4", "Date of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K3:K4", "Value of Service" + Environment.NewLine + "(Before Tax)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L2:Q2", "Rate of Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L3:M3", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N3:O3", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P3:Q3", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("L4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("M4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("N4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("O4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("P4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("Q4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R2:R4", "Total Invoice Value" + Environment.NewLine + "(14=(10+12 or 10+14+16))", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("R2:U2", "Perticulars of Payment Received", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("R3:R4", "Received" + Environment.NewLine + "At WH/RO", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("S3:S4", "C.R No. & Date", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("T3:T4", "Cheque/DD No. & Date", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("T3:T4", "SD", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("U3:U4", "Amount", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("V3:V4", "TDS", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("V2:V4", "Amount Received Against Bill (Rs.)", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("W2:W4", "Adjustment/Deduction", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("W2:W2", "Balance", DynamicExcel.CellAlignment.Middle);
                //exl.AddCell("W2", "Balance", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S2:S4", "PaymentMode", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T2:T4", "Remarks", DynamicExcel.CellAlignment.Middle);
                

                for (var i = 65; i < 85; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                }
                /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });/
                exl.AddTable<PpgRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });*/
                exl.AddTable<PpgRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 20, 12, 20,10 ,15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 10, 30 });
                var igstamt = model.Sum(o => o.ITaxAmount);
                var sgstamt = model.Sum(o => o.STaxAmount);
                var cgstamt = model.Sum(o => o.CTaxAmount);
                var totalamt = model.Sum(o => o.Total);
                exl.AddCell("M" + (model.Count + 6).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 6).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (model.Count + 6).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("R" + (model.Count + 6).ToString(), totalamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/

                exl.Save();
            }
            return excelFile;
        }

        private string RegisterofOutwardSupplyExcelCreditDebit(List<PpgRegisterOfOutwardSupplyModelCreditDebit> modelCreditDebit, decimal InvoiceAmount, decimal CRAmount)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "Principal Place of Business"
                        + Environment.NewLine + "CENTRAL WAREHOUSE"
                        + Environment.NewLine + Environment.NewLine
                        + "REGISTER OF OUTWARD SUPPLY (TAX INVOICE/BILL OF SUPPLY)";
                exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N1:O1", "BILL REGISTER", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:A4", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B2:B4", "GSTIN", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C2:C4", "Place" + Environment.NewLine + "(Name of State)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D2:D4", "Name of Customer to whom Service rendered", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E2:E4", "Period of Invoice", DynamicExcel.CellAlignment.Middle);                
                exl.MargeCell("F2:F4", "Nature of Invoice" + Environment.NewLine + "(Resv./Initial Fumigatiom/General Basic/Over & Above)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G2:G4", "HSN Code", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("H2:H4", "Rate per" + Environment.NewLine + "Bag/MT/Sqm", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I2:I4", "Credit / Debit Note No", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J2:J4", "Credit / Debit Note Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K2:M2", "Invoice Details", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K3:K4", "Invoice No.", DynamicExcel.CellAlignment.Middle);                
                exl.MargeCell("L3:L4", "Date of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M3:M4", "Value of Service" + Environment.NewLine + "(Before Tax)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N2:S2", "Rate of Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N3:O3", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P3:Q3", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R3:S3", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("N4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("O4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("P4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("Q4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("R4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("S4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T2:T4", "Total Invoice Value" + Environment.NewLine + "(14=(10+12 or 10+14+16))", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U2:U4", "Remarks", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("R2:U2", "Perticulars of Payment Received", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("R3:R4", "Received" + Environment.NewLine + "At WH/RO", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("S3:S4", "C.R No. & Date", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("T3:T4", "Cheque/DD No. & Date", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("T3:T4", "SD", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("U3:U4", "Amount", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("V3:V4", "TDS", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("V2:V4", "Amount Received Against Bill (Rs.)", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("W2:W4", "Adjustment/Deduction", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("W2:W2", "Balance", DynamicExcel.CellAlignment.Middle);
                //exl.AddCell("W2", "Balance", DynamicExcel.CellAlignment.Middle);    

                for (var i = 65; i < 86; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                }
                /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });/
                exl.AddTable<PpgRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });*/
                exl.AddTable("A", 6, modelCreditDebit, new[] { 6, 20, 20, 20, 12, 20, 10,15, 15, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 30 });
                var igstamt = modelCreditDebit.Sum(o => o.ITaxAmount);
                var sgstamt = modelCreditDebit.Sum(o => o.STaxAmount);
                var cgstamt = modelCreditDebit.Sum(o => o.CTaxAmount);
                var totalamt = modelCreditDebit.Sum(o => o.Total);
                exl.AddCell("O" + (modelCreditDebit.Count + 6).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (modelCreditDebit.Count + 6).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("S" + (modelCreditDebit.Count + 6).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("T" + (modelCreditDebit.Count + 6).ToString(), totalamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/

                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region Party Wise Unpaid Invoice
        public void PartyWiseUnpaidAmout(int PartyId, string AsOnDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.String, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PartyWiseUnpaidAmt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PartyWiseUnpaidDtl ObjDet = new PartyWiseUnpaidDtl();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDet.AsOnDate = AsOnDate.ToString();
                    ObjDet.PartyName = Result["PartyName"].ToString();
                    ObjDet.lstDtl.Add(new InvoiceDtl
                    {
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDet;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
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

        #region All Party Unpaid Invoice
        public void PartyUnpaidCreditStatus(string AsOnDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.String, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("AllPartyUnpaidAmt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PartyUnpaidDetails> lstDet = new List<PartyUnpaidDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstDet.Add(new PartyUnpaidDetails
                    {
                        PartyName = Convert.ToString(Result["PartyName"]),
                        UnpaidAmt = Convert.ToDecimal(Result["UnpaidAmt"])
                    });
                }
                if (lstDet.Count > 0)
                {
                    lstDet[0].AsOnDate = AsOnDate.ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstDet;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
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

        #region Bulk Cash receipt 
        public void GetBulkCashreceipt(string FromDate, string ToDate, string ReceiptNo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate == null ? "" : FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate == null ? "" : ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ReceiptNo });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBulkCashRecptForPrint", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[1].Rows.Count > 0)
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

        public void GetChequeBounceCashreceipt(string ReceiptNo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate == null ? "" : FromDate).ToString("yyyy-MM-dd") });
            //     LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate == null ? "" : ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ReceiptNo });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetCashRecptForCCPrint", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[1].Rows.Count > 0)
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

        #region ContainerwiseInvoiceReport

        public void ContainerInvoiceDetailsForPrint(PPGContainerInvoiceReport ObjBulkInvoiceReport)
        {

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.InvoiceId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("getContainerwiseInvoiceforprint", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[1].Rows.Count > 0)
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


        public void GetContainer()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerInvoice", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<PPGContainerInvoiceReport> objContainerLst = new List<PPGContainerInvoiceReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objContainerLst.Add(new PPGContainerInvoiceReport()
                    {
                        InvoiceId = Convert.ToInt32(Result["InoviceId"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objContainerLst;
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

      
        #region DailyCashBook
        public void DailyCashBook(string PeriodFrom, string PeriodTo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(PeriodFrom).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(PeriodTo).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyCashBookReport", CommandType.StoredProcedure, DParam);
            IList<Wlj_DailyCashBook> LstDailyCashBook = new List<Wlj_DailyCashBook>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstDailyCashBook.Add(new Wlj_DailyCashBook
                    {
                        CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),
                        Depositor = Result["Party"].ToString(),
                        ChqNo = Result["ChequeNo"].ToString(),
                        Area = Convert.ToDecimal(Result["GenSpace"]),
                        GRE = Convert.ToDecimal(Result["GRE"]),
                        GRL = Convert.ToDecimal(Result["GRL"]),
                        Reefer = Convert.ToDecimal(Result["Reefer"]),
                        INS = Convert.ToDecimal(Result["INS"]),
                        STO = Convert.ToDecimal(Result["STO"]),
                        EscCharge = Convert.ToDecimal(Result["EscCharge"]),
                        Print = Convert.ToDecimal(Result["Print"]),
                        Royality = Convert.ToDecimal(Result["Royality"]),
                        Franchiese = Convert.ToDecimal(Result["Franchiese"]),
                        HT = Convert.ToDecimal(Result["HT"]),
                        EGM = Convert.ToDecimal(Result["EGM"]),
                        Documentation = Convert.ToDecimal(Result["Documentation"]),
                        Taxable = Convert.ToDecimal(Result["Taxable"]),
                        Cgst = Convert.ToDecimal(Result["CGSTAmt"]),
                        Sgst = Convert.ToDecimal(Result["SGSTAmt"]),
                        Igst = Convert.ToDecimal(Result["IGSTAmt"]),
                        TotalCash = Convert.ToDecimal(Result["TotalCash"]),
                        TotalCheque = Convert.ToDecimal(Result["TotalCheque"]),
                        Tds = Convert.ToDecimal(Result["tdsCol"]),
                        CrTds = Convert.ToDecimal(Result["crTDS"]),
                        Roundoff = Convert.ToDecimal(Result["RoundUp"])
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDailyCashBook;
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
        public void MonthlyCashBook(MonthlyCashBook ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("MonthlyCashBookReport", CommandType.StoredProcedure, DParam);
            Wlj_MonthlyCashBook WljCashBook = new Wlj_MonthlyCashBook();

            IList<Wlj_DailyCashBook> LstDailyCashBook = new List<Wlj_DailyCashBook>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    WljCashBook.lstCashReceipt.Add(new Wlj_DailyCashBook
                    {
                        
                        ReceiptDate = Convert.ToDateTime(Result["RDate"] == DBNull.Value ? "N/A" : Result["RDate"]).ToString("dd/MM/yyyy"),
                        OpCash = Convert.ToDecimal(Result["OpCash"]),
                        OpChq = Convert.ToDecimal(Result["OpCheque"]),
                        CRNo = Result["ReceiptNo"].ToString(),
                        Depositor = Result["particular"].ToString(),
                        OTCharge = Convert.ToDecimal(Result["OTCharge"]),
                        GRE = Convert.ToDecimal(Result["GRE"]),
                        GRL = Convert.ToDecimal(Result["GRL"]),
                        STO = Convert.ToDecimal(Result["Storagech"]),
                        Print = Convert.ToDecimal(Result["Printing"]),
                        INS = Convert.ToDecimal(Result["Insurance"]),
                        Documentation = Convert.ToDecimal(Result["Document"]),
                       
                        EGM = Convert.ToDecimal(Result["EGM"]),
                        Taxable = Convert.ToDecimal(Result["Taxable"]),
                        Cgst = Convert.ToDecimal(Result["CGSTAmt"]),
                        Sgst = Convert.ToDecimal(Result["SGSTAmt"].ToString()),
                        Igst = Convert.ToDecimal(Result["IGSTAmt"].ToString()),
                        Roundoff = Convert.ToDecimal(Result["RoundUp"]),
                        TotalCash = Convert.ToDecimal(Result["TotalCash"].ToString()),
                        TotalCheque = Convert.ToDecimal(Result["TotalChq"].ToString()),
                        //  ChqNo = Result["ChequeNo"].ToString(),
                        TotalSD = Convert.ToDecimal(Result["TotalSD"]),
                        Tds = Convert.ToDecimal(Result["tdsCol"].ToString()),
                        cloCash = Convert.ToDecimal(Result["cloCash"]),

                        clochq = Convert.ToDecimal(Result["clochq"])


                       
                       
                        //Royality = Convert.ToDecimal(Result["Royality"]),
                        //Franchiese = Convert.ToDecimal(Result["Franchiese"]),
                        //HT = Convert.ToDecimal(Result["HT"]),
                       

                       
                      
                       
                        //CrTds = Result["crTDS"].ToString(),
                        
                    });
                }

                if (Result.NextResult())
                {
                    if(Result.Read())
                    {
                        WljCashBook.StartRNo = Result["StartRNo"] == DBNull.Value ? "" : Result["StartRNo"].ToString();
                        WljCashBook.LastRNo = Result["LastRNo"] == DBNull.Value ? "" : Result["LastRNo"].ToString();
                        WljCashBook.TotalRNo = Result["TotalRNo"] == DBNull.Value? 0:Convert.ToInt32(Result["TotalRNo"]);
                    }

                }
                if (Result.NextResult())
                {
                    if (Result.Read())
                    {
                        WljCashBook.OpCash = Result["OpCash"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["OpCash"]);
                        WljCashBook.OpChq = Result["OpChq"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["OpChq"]);
                        WljCashBook.TotalOp = Result["TotalOp"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalOp"]);
                        WljCashBook.ReceiptCash = Result["ReceiptCash"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["ReceiptCash"]);
                        WljCashBook.ReceiptChq = Result["ReceiptChq"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["ReceiptChq"]);
                        WljCashBook.TotalReceipt = Result["TotalReceipt"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalReceipt"]);
                        WljCashBook.CashBank = Result["CashBank"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["CashBank"]);
                        WljCashBook.ChqBank = Result["ChqBank"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["ChqBank"]);
                        WljCashBook.TotalBank = Result["TotalBank"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalBank"]);
                        WljCashBook.ClosingCash = Result["ClosingCash"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["ClosingCash"]);
                        WljCashBook.ClosingChq = Result["ClosingChq"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["ClosingChq"]);
                        WljCashBook.TotalClose = Result["TotalClose"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalClose"]);
                    
                    }

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = WljCashBook;
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

        public void InsuranceRegister(MonthlyCashBook ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("InsuranceRegisterReport", CommandType.StoredProcedure, DParam);
            Wij_InsuranceRegister WljInsReg = new Wij_InsuranceRegister();
            IList<Wij_InsuranceRegister> LsInsRes = new List<Wij_InsuranceRegister>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    DateTime DT = Convert.ToDateTime(Result["TransactionDate"].ToString());
                    LsInsRes.Add(new Wij_InsuranceRegister
                    {
                       TransactionDate = DT.ToString("dd/mm/yyyy"),
                      ReceivedUnit = Result["ReceivedUnit"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["ReceivedUnit"]),
                      ReceivedWeight = Result["ReceivedWeight"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["ReceivedWeight"]),
                       DeliveryUnit = Result["DeliveryUnit"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["DeliveryUnit"]),
                      DeliveryWeight = Result["DeliveryWeight"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["DeliveryWeight"]),
                      ClosingUnit = Result["ClosingUnit"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["ClosingUnit"]),
                       ClosingWeight = Result["ClosingWeight"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["ClosingWeight"]),
                       Item = Result["Item"].ToString(),
                       Rate = Result["Rate"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                       CommodityValue = Convert.ToDecimal(Result["ClosingWeight"]) * Convert.ToDecimal(Result["Rate"]),
                       TotalValue = Convert.ToDecimal(Result["ClosingWeight"]) * Convert.ToDecimal(Result["Rate"])



                    }
                    );

                    }

                

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LsInsRes;
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


        public void DailyPdaActivity(DailyPdaActivityReport ObjDailyPdaActivityReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyPdActivityReport", CommandType.StoredProcedure, DParam);
            IList<DailyPdaActivityReport> LstDailyPdaActivityReport = new List<DailyPdaActivityReport>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)
                    if (Result["Deposit"].ToString().Equals("0.00") && Result["Withdraw"].ToString().Equals("0.00"))
                    {

                    }
                    else
                    {
                        LstDailyPdaActivityReport.Add(new DailyPdaActivityReport
                        {
                            Date = Result["Date"].ToString(),
                            ReceiptNo = Result["ReceiptNo"].ToString(),
                            Party = Result["Party"].ToString(),
                            Deposit = Result["Deposit"].ToString(),
                            Withdraw = Result["Withdraw"].ToString()

                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDailyPdaActivityReport;
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


        public void PdSummaryReport(PdSummary ObjPdSummaryReport,int type=1)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjPdSummaryReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            // DateTime dtTo = DateTime.ParseExact(ObjCargoInStockReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_asOndate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.Int32, Value = type });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PdSummaryReport", CommandType.StoredProcedure, DParam);
            IList<PdSummary> LstPdSummaryReport = new List<PdSummary>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)

                    LstPdSummaryReport.Add(new PdSummary
                    {



                        PartyName = Result["PartyName"].ToString(),

                        Amount = Result["Amount"].ToString()

                        //ContainerNo = Result["ContainerNo"].ToString(),
                        //value = Result["value"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPdSummaryReport;
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
        public void GetPayeeNameforUnpaidInvoice()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPayeeNameforUnpaidInv", CommandType.StoredProcedure, DParam);
            List<dynamic> lstParty = new List<dynamic>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new { PartyId = Convert.ToInt32(Result["PartyId"]), PartyName = Convert.ToString(Result["PartyName"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstParty;
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

       

        #region CashReceiptInvoiceLedgerReport Partywise
        public void GetCrInvLedgerReport(int partyId, string fromdate, string todate, string comname, string address)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = partyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = fromdate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = todate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CashReceiptInvoiceLedger", CommandType.StoredProcedure, DParam);
            CashReceiptInvoiceLedger CrInvLedgerObj = new CashReceiptInvoiceLedger();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    
                    CrInvLedgerObj.OpenningBalance = Convert.ToDecimal(Result["OpenningBalance"]);
                    CrInvLedgerObj.EximTraderName= Convert.ToString(Result["EximTraderName"]);
                    CrInvLedgerObj.Address = Convert.ToString(Result["Address"]);
                    CrInvLedgerObj.City = Convert.ToString(Result["City"]);
                    CrInvLedgerObj.State = Convert.ToString(Result["State"]);
                    CrInvLedgerObj.GSTNo = Convert.ToString(Result["GSTNo"]);
                    CrInvLedgerObj.PinCode = Convert.ToString(Result["PinCode"]);
                    CrInvLedgerObj.COMGST = Convert.ToString(Result["COMGST"]);
                    CrInvLedgerObj.COMPAN = Convert.ToString(Result["COMPAN"]);
                    CrInvLedgerObj.CurDate = DateTime.Now.ToString("dd-MMM-yyyy");

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        CrInvLedgerObj.lstLedgerSummary.Add(new CrInvLedgerSummary
                        {
                            InvCr = Convert.ToInt32(Result["InvCr"]),
                            InvCrId = Convert.ToInt32(Result["InvCrId"]),
                            InvCrNo = Convert.ToString(Result["InvCrNo"]),
                            InvCrDate = Convert.ToString(Result["InvCrDate"]),
                            Debit = Convert.ToDecimal(Result["Debit"]),
                            Credit = Convert.ToDecimal(Result["Credit"]),
                            CreatedOn = Convert.ToString(Result["CreatedOn"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        CrInvLedgerObj.lstLedgerDetails.Add(new CrInvLedgerDetails
                        {
                            InvCr = Convert.ToInt32(Result["InvCr"]),
                            InvCrId = Convert.ToInt32(Result["InvCrId"]),
                            Description = Convert.ToString(Result["Description"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        CrInvLedgerObj.lstLedgerDetailsFull.Add(new CrInvLedgerFullDetails
                        {
                            Sr = Convert.ToInt32(Result["Sr"]),
                            //Sr, ReceiptDt, ReceiptNo, ChargeCode, ContNo, Size, Debit, Credit, Balance, GroupSr, InvCr, InvCrId
                            ReceiptDt = Convert.ToString(Result["ReceiptDt"]),
                            ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                            ChargeCode = Convert.ToString(Result["ChargeCode"]),
                            ContNo = Convert.ToString(Result["ContNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            Debit = Convert.ToDecimal(Result["Debit"]),
                            Credit = Convert.ToDecimal(Result["Credit"]),
                            Balance = Convert.ToDecimal(Result["Balance"]),
                            GroupSr= Convert.ToString(Result["GroupSr"]),
                          

                        });
                    }
                }
              
                if (Status == 1)
                {
                    CrInvLedgerObj.CompanyName = comname;
                    CrInvLedgerObj.CompanyAddress = address;

                    CrInvLedgerObj.lstLedgerSummary.ForEach(item =>
                    {
                        var dtls = CrInvLedgerObj.lstLedgerDetails.Where(o => o.InvCr == item.InvCr && o.InvCrId == item.InvCrId).ToList();
                        dtls.ForEach(d =>
                        {
                            item.LedgerDetails.Add(d);
                        });
                    });

                    CrInvLedgerObj.TotalDebit = CrInvLedgerObj.lstLedgerDetailsFull.Sum(x => x.Debit);
                    CrInvLedgerObj.TotalCredit = CrInvLedgerObj.lstLedgerDetailsFull.Sum(x => x.Credit);
                    CrInvLedgerObj.ClosingBalance = CrInvLedgerObj.OpenningBalance + CrInvLedgerObj.TotalCredit - CrInvLedgerObj.TotalDebit;
                    /* CrInvLedgerObj.ClosingBalance = CrInvLedgerObj.lstLedgerBalance.Sum(x => x.Total);*/


                    /*  CrInvLedgerObj.ClosingBalance = (CrInvLedgerObj.OpenningBalance + (CrInvLedgerObj.lstLedgerSummary.Sum(o => o.Debit)))
                                                      - (CrInvLedgerObj.lstLedgerSummary.Sum(o => o.Credit));*/


                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CrInvLedgerObj;
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

        #region WorkSlip
        public void WorkSlipDetailsForPrint(string PeriodFrom, string PeriodTo,int CasualLabour,int Uid=0)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodFrom) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodTo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Labour", MySqlDbType = MySqlDbType.Int32, Value = CasualLabour });
            LstParam.Add(new MySqlParameter { ParameterName = "in_uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetWorkslipReport", CommandType.StoredProcedure, DParam);
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
        #region Account Report Export Cargo In General Carting
        public void GetCargoExport(Ppg_CarGenCar objPC)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPC.AsOnDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ExpCarGenCarReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Ppg_ExpCarGen> lstPV = new List<Ppg_ExpCarGen>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_ExpCarGen
                    {
                        EntryNo = Result["EntryNo"].ToString(),
                        InDate = Result["InDate"].ToString(),
                        SbNo = (Result["SbNo"]).ToString(),
                        SbDate = Result["SbDate"].ToString(),
                        Shed = Result["Shed"].ToString(),
                        Area = Convert.ToDecimal(Result["Area"]),
                        NoOfDays = Convert.ToInt32(Result["NoOfDays"]),
                        NoOfWeek = Convert.ToInt32(Result["NoOfWeek"]),
                        GeneralAmount = Convert.ToDecimal(Result["GeneralAmount"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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

        #region PV Report
        public void GetPVReportImport(Ppg_PV objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Ppg_ImpPVReport> lstPV = new List<Ppg_ImpPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_ImpPVReport
                    {
                        BOLNo = Result["BOLNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        CommodityAlias = Result["CommodityAlias"].ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        NoOfUnitsRec = Convert.ToInt32(Result["NoOfUnitsRec"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        Area = Convert.ToDecimal(Result["Area"].ToString()),
                        LocationName = Result["LocationName"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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
        public void GetPVReportExport(Ppg_PV objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Ppg_ExpPVReport> lstPV = new List<Ppg_ExpPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_ExpPVReport
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        ShippingBillDate = Result["ShippingBillDate"].ToString(),
                        EntryNo = Result["EntryNo"].ToString(),
                        RegisterDate = Result["RegisterDate"].ToString(),
                        Units = Convert.ToInt32(Result["Units"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        Fob = Convert.ToDecimal(Result["Fob"]),
                        Area = Convert.ToDecimal(Result["Area"]),
                        EximTraderName = Result["EximTraderName"].ToString(),
                        EximTraderAlias = Result["EximTraderAlias"].ToString(),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        LocationName = Result["LocationName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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

        #region Daily Transaction Report
        public void DTRForPrint(string DTRDate, int GodownId = 0)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("DailyTransactionReport", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[0].Rows.Count > 0)
                {
                    Status = 1;
                }
                else if (Result != null && Result.Tables[1].Rows.Count > 0)
                {
                    Status = 1;
                }
                else
                {
                    Status = 0;
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
        public void DTRForExport(string DTRDate, int GodownId = 0)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            _DBResponse = new DatabaseResponse();
            try
            {
                DParam = LstParam.ToArray();
                //Result = DataAccess.ExecuteDataSet("DailyTransactionExp", CommandType.StoredProcedure, DParam);
                PpgDTRExp obj = new PpgDTRExp();
                obj =(PpgDTRExp) DataAccess.ExecuteDynamicSet<PpgDTRExp>("DailyTransactionExp", DParam);
                if(obj.lstBTTDetails.Count>0 || obj.lstCargoAccepting.Count > 0 || obj.lstCargoShifting.Count > 0 || obj.lstCartingDetails.Count > 0||
                    obj.lstStuffingDetails.Count > 0|| obj.StockOpening.Count > 0|| obj.StockClosing.Count > 0 || obj.lstShortCartingDetails.Count > 0)
                    _DBResponse.Status = 1;
                else
                    _DBResponse.Status = 0;
                _DBResponse.Message = "Data";
                _DBResponse.Data = obj;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        #endregion

        #region SealCuttingReport
        public void SealCuttingReport(SealCuttingReport SealCuttingReport)
        {

            DateTime dtfrom = DateTime.ParseExact(SealCuttingReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(SealCuttingReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            DataSet Result = new DataSet();
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });

            DParam = LstParam.ToArray();
            Result = DataAccess.ExecuteDataSet("GetSealCuttingReport", CommandType.StoredProcedure, DParam);
            IList<SealCuttingReport> LstSealClosingReport = new List<SealCuttingReport>();
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result != null && Result.Tables[0].Rows.Count > 0)
                {
                    Status = 1;
                }
                else
                {
                    Status = 0;
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


        #region Collection Report
        public void CollectionReport(Ppg_CollectionReport ObjCollectionReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjCollectionReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjCollectionReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CollectionReport", CommandType.StoredProcedure, DParam);
            Ppg_FinalCollectionReportTotal LstCollectionReport = new Ppg_FinalCollectionReportTotal();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)

                    LstCollectionReport.listCollectionReport.Add(new Ppg_CollectionReport
                    {
                        Date = Result["DateFormatted"].ToString(),
                        Cash = Result["Cash"].ToString(),
                        Bank = Result["Cheque"].ToString(),//bank has data for cheque see view
                        PO = Result["PO"].ToString(),
                        DD = Result["DRAFT"].ToString(),
                        Pd = Result["NEFT"].ToString(),//neft has data for pd see view
                        Chln = Result["CHALLAN"].ToString(),
                        Total = Result["Total"].ToString()

                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstCollectionReport.objCollectionReportTotal.TotalPDA = Convert.ToString(Result["TotalNEFT"]);
                        LstCollectionReport.objCollectionReportTotal.TotalCash = Convert.ToString(Result["TotalCash"]);
                        LstCollectionReport.objCollectionReportTotal.TotalBank = Convert.ToString(Result["TotalCheque"]);

                        LstCollectionReport.objCollectionReportTotal.TotalPO = Convert.ToString(Result["TotalPO"]);
                        LstCollectionReport.objCollectionReportTotal.TotalCHALLAN = Convert.ToString(Result["TotalCHALLAN"]);
                        LstCollectionReport.objCollectionReportTotal.TotalDraft = Convert.ToString(Result["TotalDRAFT"]);



                    }
                }
                if (Status == 1)
                {
                    if (LstCollectionReport.listCollectionReport.Count > 0)
                    {
                        LstCollectionReport.listCollectionReport.Add(new Ppg_CollectionReport
                        {
                            Date = "Total",
                            Cash = LstCollectionReport.listCollectionReport.Sum(o => Convert.ToDecimal(o.Cash)).ToString(),
                            Bank = LstCollectionReport.listCollectionReport.Sum(o => Convert.ToDecimal(o.Bank)).ToString(),
                            PO = LstCollectionReport.listCollectionReport.Sum(o => Convert.ToDecimal(o.PO)).ToString(),
                            DD = LstCollectionReport.listCollectionReport.Sum(o => Convert.ToDecimal(o.DD)).ToString(),
                            Pd = LstCollectionReport.listCollectionReport.Sum(o => Convert.ToDecimal(o.Pd)).ToString(),
                            Chln = LstCollectionReport.listCollectionReport.Sum(o => Convert.ToDecimal(o.Chln)).ToString(),
                            Total = LstCollectionReport.listCollectionReport.Sum(o => Convert.ToDecimal(o.Total)).ToString()

                        });
                    }
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCollectionReport;
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

        #region Accounts Report for Import Cargo
        public void GetImpCargo(Ppg_PV objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpCargoReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Ppg_ImpPVReport> lstPV = new List<Ppg_ImpPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_ImpPVReport
                    {
                        BOLNo = Result["BOLNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        CommodityAlias = Result["CommodityAlias"].ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        NoOfUnitsRec = Convert.ToInt32(Result["NoOfUnitsRec"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        Area = Convert.ToDecimal(Result["Area"].ToString()),
                        LocationName = Result["LocationName"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        TotWk = Convert.ToInt32(Result["TotWk"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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

        #region Cheque Summary Report
        public void ChequeSummary(Ppg_ChequeSummary ObjChequeSummary)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjChequeSummary.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjChequeSummary.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = ObjChequeSummary.Type });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ChequeSummaryReport", CommandType.StoredProcedure, DParam);
            IList<Ppg_ChequeSummary> LstChequeSummary = new List<Ppg_ChequeSummary>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstChequeSummary.Add(new Ppg_ChequeSummary
                    {



                        Bank = Result["DraweeBank"].ToString(),

                        Amount = Result["Amount"].ToString(),

                        ChequeNumber = Result["chequeNumber"].ToString(),
                        Date = Result["Date"].ToString(),
                        Party = Result["Party"].ToString(),
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString()
                        //value = Result["value"].ToString()

                    });
                }
                if (Status == 1)
                {
                    LstChequeSummary.Add(new Ppg_ChequeSummary
                    {



                        Bank = string.Empty,

                        Amount = LstChequeSummary.ToList().Sum(m => Convert.ToDecimal(m.Amount)).ToString(),

                        ChequeNumber = "<strong>Total</strong>",
                        Date = string.Empty,
                        Party = string.Empty,
                        InvoiceNumber = string.Empty,
                        ReceiptNo = string.Empty
                        //value = Result["value"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstChequeSummary;
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

        public void ChequeCashDDPOSummary(Ppg_CashChequeDDSummary ObjChequeSummary)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjChequeSummary.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjChequeSummary.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = ObjChequeSummary.Type });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CashCqeDdPoSdStmtRpt", CommandType.StoredProcedure, DParam);
            IList<Ppg_CashChequeDDSummary> LstChequeSummary = new List<Ppg_CashChequeDDSummary>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstChequeSummary.Add(new Ppg_CashChequeDDSummary
                    {



                        Bank = Result["DraweeBank"].ToString(),
                        Type= Result["PayMode"].ToString(),
                        CashAmount=Convert.ToDecimal(Result["CASHAmount"]),
                        ChequeAmount=  Convert.ToDecimal(Result["CHEQUEAmount"]) ,
                        POSAmount=  Convert.ToDecimal(Result["POAmount"]) ,
                        Amount =  Result["OtherAmount"].ToString(),
                        SDAmount = Convert.ToDecimal(Result["SDAmount"]),


                        GCashAmount = Convert.ToDecimal(Result["CASHAmount"]),
                        GChequeAmount = Convert.ToDecimal(Result["CHEQUEAmount"]),
                        GPOSAmount = Convert.ToDecimal(Result["POAmount"]),
                        GOthersAmount = Convert.ToDecimal(Result["OtherAmount"]),

                        ChequeNumber = Result["chequeNumber"].ToString(),
                        Date = Result["Date"].ToString(),
                        Party = Result["Party"].ToString(),
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString()
                        //value = Result["value"].ToString()

                    });
                }
                if (Status == 1)
                {
                    LstChequeSummary.Add(new Ppg_CashChequeDDSummary
                    {



                        Bank = string.Empty,

                        GOthersAmount = LstChequeSummary.ToList().Sum(m => Convert.ToDecimal(m.GOthersAmount)),
                        GCashAmount = LstChequeSummary.ToList().Sum(m => m.GCashAmount),
                        GChequeAmount = LstChequeSummary.ToList().Sum(m => m.ChequeAmount),
                        GPOSAmount = LstChequeSummary.ToList().Sum(m => m.GPOSAmount),
                        SDAmount = LstChequeSummary.ToList().Sum(m => m.SDAmount),

                        Amount = string.Empty,


                        ChequeNumber = "<strong>Total</strong>",
                        Date = string.Empty,
                        Party = string.Empty,
                        InvoiceNumber = string.Empty,
                        ReceiptNo = string.Empty
                        //value = Result["value"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstChequeSummary;
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

        #region PV Report Of Empty Container
        public void GetEmptyCont(Ppg_PVReport objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReportEmptyCont", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Ppg_PVReportImpEmptyCont> lstPV = new List<Ppg_PVReportImpEmptyCont>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_PVReportImpEmptyCont
                    {
                        CFSCode = Result["EntryNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        EntryDateTime = Result["InDate"].ToString(),
                        Size = Result["Size"].ToString(),
                        EximTraderAlias = Result["SlaCd"].ToString(),
                        Days = Convert.ToInt32(Result["Days"]),
                        Amount = Convert.ToDecimal(Result["GRE"]),
                        InDateEcy = Result["InDateEcy"].ToString(),
                        OutDateEcy = Result["OutDate"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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

        #region PV Report Of Import Loaded Container
        public void GetImpLoadedCont(Ppg_PVReport objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReportImpLoadedCont", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Ppg_PVReportImpLoadedCont> lstPV = new List<Ppg_PVReportImpLoadedCont>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_PVReportImpLoadedCont
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        EximTraderAlias = Result["EximTraderAlias"].ToString(),
                        Days = Convert.ToInt32(Result["Days"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        TransportFrom = Result["TransportFrom"].ToString(),
                        Size = Result["Size"].ToString(),
                        Remarks= Result["Remarks"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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

        #region Bulk Credit Note print
        public void BulkCreditNoteprint(string InvoiceNumber, int PartyId, DateTime FromDate, DateTime ToDate)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CRNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("getBulkCRNoteDetailsForPrint", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[1].Rows.Count > 0)
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

        #region Daily Invoice Report
        public void DailyInvoiceReport(Ppg_DailyInvReport ObjDailyReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyReport.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyReport.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.String, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.String, Value = PeriodTo });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjTDSReport.PartyId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyInvoiceReport", CommandType.StoredProcedure, DParam);
            IList<Ppg_DailyReport> lstPV = new List<Ppg_DailyReport>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_DailyReport
                    {
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        Party = Result["PartyName"].ToString(),
                        TotalAmt = Convert.ToDecimal(Result["TotalAmt"]),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        ReceiptAmt = Convert.ToDecimal(Result["Amount"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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

        #region SDSummary
        public void SDSummary(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("SDSummary", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<SDSummary> lstPV = new List<SDSummary>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new SDSummary
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        Date=Convert.ToString(Result["InvoiceDate"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        Module = Result["InvoiceType"].ToString(),
                        EximTraderName = Result["PartyName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),

                        BILL = Convert.ToDecimal(Result["MiscExcess"]),
                        GEN = Convert.ToDecimal(Result["GenSpace"]),
                        STO = Convert.ToDecimal(Result["sto"]),
                        INS = Convert.ToDecimal(Result["Insurance"]),
                        GRE = Convert.ToDecimal(Result["GroundRentEmpty"]),
                        GRL = Convert.ToDecimal(Result["GroundRentLoaded"]),
                        MFCHRG = Convert.ToDecimal(Result["Mf"]),
                        //MFTAX = Convert.ToDecimal(Result["MFTAX"]),
                        PDA = Convert.ToDecimal(Result["TotalPDA"]),
                        ENT = Convert.ToDecimal(Result["EntCharge"]),
                        FUM = Convert.ToDecimal(Result["Fum"]),
                        OT = Convert.ToDecimal(Result["OtCharge"]),
                        CGST = Convert.ToDecimal(Result["CGSTAmt"]),
                        SGST = Convert.ToDecimal(Result["SGSTAmt"]),
                        IGST = Convert.ToDecimal(Result["IGSTAmt"]),
                        MISC = Convert.ToDecimal(Result["MISC"]),
                        Total = Convert.ToDecimal(Result["TotalPDA"]),
                        TDS = Convert.ToString(Result["tdsCol"]),
                        CRTDS = Convert.ToDecimal(Result["crTDS"]),
                        PaymentType = Result["ModeOfPay"].ToString(),
                        Remarks = Result["Remarks"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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

        #region Satement Of Empty Container
        public void StatementOfEmptyContainer(PpgStatementOfEmptyContainer ObjStatementOfEmptyContainer)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjStatementOfEmptyContainer.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjStatementOfEmptyContainer.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("StatementOfEmptyContainer", CommandType.StoredProcedure, DParam);
            IList<PpgStatementOfEmptyContainer> LstStatementOfEmptyContainer = new List<PpgStatementOfEmptyContainer>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)

                    LstStatementOfEmptyContainer.Add(new PpgStatementOfEmptyContainer
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        DateOfArrival = Result["DateOFarrival"].ToString(),
                        ImportExport = Result["OperationType"].ToString(),
                        dateofdetuffing = Result["DateOFDestuffing"] == DBNull.Value ? "" : Result["DateOFDestuffing"].ToString()

                        // Withdraw = Result["Withdraw"].ToString()//OperationType

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStatementOfEmptyContainer;
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

        #region GodownList according to Godown Rights
        public void GetGodownRightsWise(int Uid)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGodownListAccdRights", CommandType.StoredProcedure, DParam);
            IList<GodownList> lst = new List<GodownList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lst.Add(new GodownList
                    {
                        GodownName = Result["GodownName"].ToString(),
                        GodownId = Convert.ToInt32(Result["GodownId"])
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lst;
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
        #region Export RR report
        public void GetContainerForExportRR()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForExportRR", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            List<PPGExportRRReport> LstStuffing = new List<PPGExportRRReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    if (LstStuffing.Count <= 0)
                    {
                        LstStuffing.Add(new PPGExportRRReport
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString()
                        });
                    }

                    else
                    {
                        int flag = 0;

                        for (int i1 = 0; i1 < LstStuffing.Count; i1++)
                        {
                            if (LstStuffing[i1].ContainerNo == Result["ContainerNo"].ToString())
                            {
                                flag = 1;
                                break;
                            }
                        }

                        if (flag == 0)
                        {
                            LstStuffing.Add(new PPGExportRRReport
                            {
                                ContainerNo = Result["ContainerNo"].ToString(),
                                CFSCode = Result["CFSCode"].ToString()
                            });
                        }
                    }
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



        public void PrintExportRR(String ContNo, String CfsCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContNo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CfsCode });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintExportRR", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PrintExportRRReport ObjDeliveryOrder = new PrintExportRRReport();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDeliveryOrder.TrainNo = Result["TrainNo"].ToString();
                    ObjDeliveryOrder.TrainDate = Result["TrainDate"].ToString();
                    ObjDeliveryOrder.InvoiceNo = Convert.ToString(Result["InvoiceNo"]);
                    ObjDeliveryOrder.CFS_Port = Convert.ToString(Result["CFS_Port"]);
                    ObjDeliveryOrder.PortCode = Convert.ToString(Result["PortCode"]);
                    ObjDeliveryOrder.SLine = Convert.ToString(Result["SLine"]);

                    ObjDeliveryOrder.CustomSeal = Result["CustomSeal"].ToString();
                    ObjDeliveryOrder.TW = Convert.ToDecimal(Result["TW"].ToString());
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstPartyDetails.Add(new ppgPartyDetails
                        {
                            PartyName = Result["PartyName"].ToString(),
                            PartyAddress = Result["PartyAddress"].ToString(),
                            PayeeName = Result["PayeeName"].ToString(),
                            PayeeAddress = Result["PayeeAddress"].ToString()

                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstContDetails.Add(new ppgContDetails
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            Loaded = Result["Loaded"].ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"])

                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstChargesDetails.Add(new ppgChargesDetails
                        {
                            RR = Convert.ToDecimal(Result["RR"]),
                            THC = Convert.ToDecimal(Result["THC"]),
                            CGST = Convert.ToDecimal(Result["CGST"].ToString()),
                            SGST = Convert.ToDecimal(Result["SGST"].ToString()),
                            IGST = Convert.ToDecimal(Result["IGST"].ToString()),
                            Inv = Convert.ToDecimal(Result["Inv"].ToString())
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstCompDetails.Add(new ppgCompDetails
                        {
                            Pan = Result["Pan"].ToString(),
                            GstIn = Result["GstIn"].ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstReceiptDetails.Add(new ppgReceiptDetails
                        {
                            InvoiceNo = Result["InvoiceNo"].ToString(),
                            ReceiptNo = Result["ReceiptNo"].ToString(),
                            ReceiptDate = Result["ReceiptDate"].ToString(),
                            InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"].ToString()),
                            PayeeName = Result["PayeeName"].ToString(),

                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDeliveryOrder;
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


        #region SD Details Statement
        public void GetAllPartyForSDDet(string PartyCode, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyForSDDet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PartyForSDDet> LstParty = new List<PartyForSDDet>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstParty.Add(new PartyForSDDet
                    {
                        Party = Result["Party"].ToString(),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
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
                    _DBResponse.Data = new { LstParty, State };
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

        public void GetSDDetStatement(int PartyId, string Fdt, String Tdt)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SDPartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fdt", MySqlDbType = MySqlDbType.Date, Value = Fdt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Tdt", MySqlDbType = MySqlDbType.Date, Value = Tdt });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSDDetailsStatement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PpgSDDetailsStatement SDResult = new PpgSDDetailsStatement();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    SDResult.PartyName = Result["PartyName"].ToString();
                    SDResult.PartyCode = Result["PartyCode"].ToString();
                    SDResult.PartyGst = Result["PartyGst"].ToString();
                    SDResult.CompanyGst = Result["CompanyGst"].ToString();
                    //SDResult.UtilizationAmount = Convert.ToDecimal(Result["UtilizationAmount"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        SDResult.lstInvc.Add(
                            new PpgSDInvoiceDet
                            {
                                //Sl, InvoiceNo, InvoiceDate, ReceiptNo, ReceiptDate, ReceiptAmt, TranType, TranAmt
                                SL = Convert.ToInt32(Result["Sl"]),
                                InvoiceNo = Result["InvoiceNo"].ToString(),
                                InvoiceDate = Result["InvoiceDate"].ToString(),
                                ReceiptNo = Result["ReceiptNo"].ToString(),
                                ReceiptDate = Result["ReceiptDate"].ToString(),
                                ReceiptAmt = Convert.ToDecimal(Result["ReceiptAmt"]),
                                TranType = Result["TranType"].ToString(),
                                TranAmt = Convert.ToDecimal(Result["TranAmt"]),


                            });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        SDResult.UtilizationAmount = Convert.ToDecimal(Result["UtilizationAmount"]);
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

        #region-- VC REPORT --
        public void VCCapacityDetails(string dtArray)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_Xml", MySqlDbType = MySqlDbType.Text, Value = dtArray });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //var Result1 = DataAccess.ExecuteDynamicSet<VCCapacityModel>("GetVCCapacityDetails", DParam);
            IDataReader Result = DataAccess.ExecuteDataReader("GetVCCapacityDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PpgVCCapacityModel> LstVCCapacity = new List<PpgVCCapacityModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstVCCapacity.Add(new PpgVCCapacityModel
                    {
                        //vccapid = (Result["vccapid"] == DBNull.Value ? 0 : Convert.ToInt32(Result["vccapid"])),
                        //vcoptdate = (Result["vcoptdate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(Result["vcoptdate"])),
                        cfscap = (Result["cfscap"] == DBNull.Value ? 496034M : Convert.ToDecimal(Result["cfscap"])),
                        bndcap = (Result["bndcap"] == DBNull.Value ? 8766M : Convert.ToDecimal(Result["bndcap"])),
                        cfsutlz = (Result["cfsutlz"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["cfsutlz"])),
                        bndutlz = (Result["bndultz"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["bndultz"])),
                        //weekid = Result["weekid"].ToString()
                    });
                }
                if (LstVCCapacity.Count() < 3)
                {
                    for (var i = LstVCCapacity.Count; i < 3; i++)
                    {
                        LstVCCapacity.Add(new PpgVCCapacityModel());
                    }
                }
                if (Result.NextResult())
                {
                    var row = 0;
                    while (Result.Read())
                    {
                        LstVCCapacity[row].Occupency = (Result["occupency"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["occupency"]));
                        LstVCCapacity[row].Income = (Result["income"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["income"]));
                        row += 1;
                    }
                }
                if (Result.NextResult())
                {
                    var row = 0;
                    while (Result.Read())
                    {
                        LstVCCapacity[row].Import = (Result["Import"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Import"]));
                        LstVCCapacity[row].Export = (Result["Export"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Export"]));
                        LstVCCapacity[row].Empty = (Result["Empty"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Empty"]));
                        row += 1;
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstVCCapacity;
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

      
        public void VCCapacityReport(string dtArray)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_Xml", MySqlDbType = MySqlDbType.Text, Value = dtArray });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //var Result1 = DataAccess.ExecuteDynamicSet<VCCapacityModel>("GetVCCapacityDetails", DParam);
            IDataReader Result = DataAccess.ExecuteDataReader("GetVCCapacityReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PpgVCCapacityModel> LstVCCapacity = new List<PpgVCCapacityModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstVCCapacity.Add(new PpgVCCapacityModel
                    {
                        Occupency = (Result["occupency"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["occupency"])),
                        Income = (Result["income"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["income"]))
                    });
                }
                if (LstVCCapacity.Count() < 3)
                {
                    for (var i = LstVCCapacity.Count; i < 3; i++)
                    {
                        LstVCCapacity.Add(new PpgVCCapacityModel());
                    }
                }
                /*if (Result.NextResult())
                {
                    var row = 0;
                    while (Result.Read())
                    {
                        LstVCCapacity[row].Occupency = (Result["occupency"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["occupency"]));
                        LstVCCapacity[row].Income = (Result["income"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["income"]));
                        row += 1;
                    }
                }*/
                if (Result.NextResult())
                {
                    var row = 0;
                    while (Result.Read())
                    {
                        LstVCCapacity[row].Import = (Result["Import"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Import"]));
                        LstVCCapacity[row].Export = (Result["Export"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Export"]));
                        LstVCCapacity[row].Empty = (Result["Empty"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Empty"]));
                        row += 1;
                    }
                }
                if (Result.NextResult())
                {
                    var row = 0;
                    while (Result.Read())
                    {
                        LstVCCapacity[row].CurYear = (Result["CurYear"] == DBNull.Value ? 0 : Convert.ToInt32(Result["CurYear"]));
                        LstVCCapacity[row].PreYear = (Result["PreYear"] == DBNull.Value ? 0 : Convert.ToInt32(Result["PreYear"]));
                        row += 1;
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstVCCapacity;
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
        #region-- LandingCertificate --
        public void GetLandingCertificateReport(String TrainNo, String TrainDate)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainNo", MySqlDbType = MySqlDbType.String, Value = TrainNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainDate", MySqlDbType = MySqlDbType.DateTime, Value =Convert.ToDateTime(TrainDate) });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetLandingCertificateReport", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();
                TrainDetl trndtl = new TrainDetl();
                int Sr = 0;
                if (Result != null && Result.Tables.Count > 0)
                {
                    trndtl.TrainNo = Result.Tables[0].Rows[0]["TrainNo"].ToString();
                    trndtl.TrainDate = Result.Tables[0].Rows[0]["TrainDate"].ToString();
                    trndtl.PortName = Result.Tables[0].Rows[0]["PortName"].ToString();
                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        Status = 1;
                        trndtl.objTrainSummaryUpload.Add(new LandingCerificate
                        {
                            SrNo = Sr + 1,
                            Container_No = Convert.ToString(dr["Container_No"]),
                            CT_Size = Convert.ToString(dr["CT_Size"]),
                            S_Line = Convert.ToString(dr["S_Line"]),
                            TP_No = Convert.ToString(dr["Smtp_No"]),
                            TP_Date = Convert.ToString(dr["Smtp_Date"]),
                            ArrivalDate = Convert.ToString(dr["ArrivalDate"]),
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = trndtl;
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

        public void GetAllTrainNo()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAllTrainNoForLC", CommandType.StoredProcedure, dpram);
            IList<PPG_TrainList> lstTrainNo = new List<PPG_TrainList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {

                    Status = 1;
                    lstTrainNo.Add(new PPG_TrainList { TrainNo = result["TrainNo"].ToString(), TrainSummaryID = Convert.ToInt32(result["TrainSummaryID"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstTrainNo;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        public void GetTrainDateByTrainNo(String TrainNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = TrainNo });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetTrainDateByTrainNo", CommandType.StoredProcedure, dpram);
            IList<PPG_TrainDateList> lstTrainDate = new List<PPG_TrainDateList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {

                    Status = 1;
                    lstTrainDate.Add(new PPG_TrainDateList { TrainDate = result["TrainDate"].ToString()});
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstTrainDate;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        #endregion

        #region Long Standing Report For Container
        public void GetLongStandingImpLoadedCont(Ppg_LongStandingImpCon ObjContainerBalanceInCFS)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjContainerBalanceInCFS.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(ObjContainerBalanceInCFS.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_days", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjContainerBalanceInCFS.days });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("LongStandingImpLoadedCont", CommandType.StoredProcedure, DParam);
            IList<Ppg_LongStandingImpConDtl> LstContainerBalanceInCFS = new List<Ppg_LongStandingImpConDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstContainerBalanceInCFS.Add(new Ppg_LongStandingImpConDtl
                    {
                        BOLNo = Result["OBL_NO"].ToString(),
                        IGMNo = Result["IGM_No"].ToString(),
                        IGMDate = Result["IGM_Date"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ImporterName = Result["ImporterName"].ToString(),
                        ImporterAddress = Result["ImporterAddress"].ToString(),
                        InDate = Result["EntryDateTime"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                        SlaCd = Result["SlaCd"].ToString(),
                        NoOfPkg =Convert.ToInt32(Result["NO_PKG"].ToString()),
                        GrWt = Convert.ToDecimal(Result["GR_WT"].ToString()),
                        Commodity = Result["CommodityName"].ToString(),
                        Notice1 = Result["Notice1"].ToString(),
                        Notice2 = Result["Notice2"].ToString(),
                        Date1 = Result["Date1"].ToString(),
                        Date2 = Result["Date2"].ToString(),
                        Nocr = Result["Nocr"].ToString(),
                        Remarks1 = Result["Remarks1"].ToString(),
                        Remarks2 = Result["Remarks2"].ToString(),
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainerBalanceInCFS;
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

      

        #region Long Standing Report For Cargo
        public void GetLongStandingImpLoadedCargo(Ppg_LongStandingImpCargo ObjContainerBalanceInCFS)
        {

            
            DateTime dtTo = DateTime.ParseExact(ObjContainerBalanceInCFS.AsOnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = ObjContainerBalanceInCFS.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_days", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjContainerBalanceInCFS.days });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("LongStandingImpLoadedCargo", CommandType.StoredProcedure, DParam);
            IList<Ppg_LongStandingImpCargoDtl> LstContainerBalanceInCFS = new List<Ppg_LongStandingImpCargoDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstContainerBalanceInCFS.Add(new Ppg_LongStandingImpCargoDtl
                    {
                        BOLNo = Result["OBL_NO"].ToString(),
                        IGMNo = Result["IGM_No"].ToString(),
                        IGMDate = Result["IGM_Date"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        DstfDate = Result["DestuffingEntryDate"].ToString(),
                        ImporterName = Result["ImporterName"].ToString(),
                        ImporterAddress = Result["ImporterAddress"].ToString(),
                        InDate = Result["EntryDateTime"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                        SlaCd = Result["SlaCd"].ToString(),
                        NoOfPkg = Convert.ToInt32(Result["NO_PKG"].ToString()),
                        GrWt = Convert.ToDecimal(Result["GR_WT"].ToString()),
                        Area = Convert.ToDecimal(Result["Area"].ToString()),
                        Commodity = Result["CommodityName"].ToString(),
                        Notice1 = Result["Notice1"].ToString(),
                        Notice2 = Result["Notice2"].ToString(),
                        Date1 = Result["Date1"].ToString(),
                        Date2 = Result["Date2"].ToString(),
                        Nocr = Result["Nocr"].ToString(),
                        Remarks1 = Result["Remarks1"].ToString(),
                        Remarks2 = Result["Remarks2"].ToString(),
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainerBalanceInCFS;
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

        #region Export Job Order report
        public void GetContainerForExportJobOrder(String Type)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForExportJobOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPGExportRRReport> LstStuffing = new List<PPGExportRRReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    if (LstStuffing.Count <= 0)
                    {
                        LstStuffing.Add(new PPGExportRRReport
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString()
                        });
                    }

                    else
                    {
                        int flag = 0;

                        for (int i1 = 0; i1 < LstStuffing.Count; i1++)
                        {
                            if (LstStuffing[i1].ContainerNo == Result["ContainerNo"].ToString())
                            {
                                flag = 1;
                                break;
                            }
                        }

                        if (flag == 0)
                        {
                            LstStuffing.Add(new PPGExportRRReport
                            {
                                ContainerNo = Result["ContainerNo"].ToString(),
                                CFSCode = Result["CFSCode"].ToString()
                            });
                        }
                    }
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



        public void PrintExportJobOrder(String PFrom, String PTo, String Ttype)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(PFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(PTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Ttype", MySqlDbType = MySqlDbType.VarChar, Value = Ttype });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("GetDetForPrntjoborder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CwcExim.Areas.Export.Models.PPGPrintJOModel objMdl = new CwcExim.Areas.Export.Models.PPGPrintJOModel();
            _DBResponse = new DatabaseResponse();
            List<CwcExim.Areas.Export.Models.PPGPrintJOModelDet> lstJobOrder = new List<CwcExim.Areas.Export.Models.PPGPrintJOModelDet>();
            try
            {

                while (result.Read())
                {
                    Status = 1;
                    lstJobOrder.Add(new CwcExim.Areas.Export.Models.PPGPrintJOModelDet
                    {
                        ContainerNo = result["ContainerNo"].ToString(),
                        ContainerSize = result["SZ"].ToString(),
                        ShippingLineName = result["ShippingLineName"].ToString(),
                        OnBehalf = result["OnBehalf"].ToString(),
                        CustomSealNo = result["CustomSeal"].ToString(),

                        Sline = result["LineSeal"].ToString(),
                        POL = result["POL"].ToString(),
                        POD = result["POD"].ToString(),
                        Ct_Tare = Convert.ToDecimal(result["TW"].ToString()),
                        Cargo_Wt = Convert.ToDecimal(result["CW"].ToString()),
                        CFSCode = result["CFSCode"].ToString(),


                        CargoType = result["CargoType"].ToString(),
                        ContainerLoadType = result["ContainerLoadType"].ToString(),
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstJobOrder;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
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
                result.Dispose();
                result.Close();
            }
        }

        public void PrintExportJoSum(String PFrom, String PTo, String Ttype)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(PFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(PTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Ttype", MySqlDbType = MySqlDbType.VarChar, Value = Ttype });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("GetDetForPrntjosummary", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CwcExim.Areas.Export.Models.PPGPrintJOModel objMdl = new CwcExim.Areas.Export.Models.PPGPrintJOModel();
            _DBResponse = new DatabaseResponse();
            List<CwcExim.Areas.Export.Models.ppgexportjobordersum> lstJobOrder = new List<CwcExim.Areas.Export.Models.ppgexportjobordersum>();
            try
            {

                while (result.Read())
                {
                    Status = 1;
                    lstJobOrder.Add(new CwcExim.Areas.Export.Models.ppgexportjobordersum
                    {

                        POL = result["POL"].ToString(),
                        SZ20 = result["SZ20"] == System.DBNull.Value?0:Convert.ToInt32(result["SZ20"].ToString()),
                        SZ40 = result["SZ40"] == System.DBNull.Value ? 0 : Convert.ToInt32(result["SZ40"].ToString())
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstJobOrder;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
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
                result.Dispose();
                result.Close();
            }
        }
        #endregion

        #region core data report
        public void PrintCoreData(String Fdt,String ToDt)
        {

           
            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Fdt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = ToDt });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjTDSReport.PartyId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CoreDataReport", CommandType.StoredProcedure, DParam);
            PPGCoreData objCoreData= new PPGCoreData();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objCoreData.ICDCash = Convert.ToDecimal(Result["ICDCash"].ToString());
                    objCoreData.ICDBill = Convert.ToDecimal(Result["ICDBill"].ToString());
                    objCoreData.ICDTotal = Convert.ToDecimal(Result["ICDTotal"].ToString());
                    objCoreData.DESSCash = Convert.ToDecimal(Result["DESSCash"].ToString());
                    objCoreData.DESSTotal = Convert.ToDecimal(Result["DESSTotal"].ToString());
                    objCoreData.CFSCash = Convert.ToDecimal(Result["CFSCash"].ToString());
                    objCoreData.IRRCash = Convert.ToDecimal(Result["IRRCash"].ToString());
                    objCoreData.IRRTotal = Convert.ToDecimal(Result["IRRTotal"].ToString());
                    objCoreData.GrossCash = Convert.ToDecimal(Result["GrossCash"].ToString());
                    objCoreData.GrossBill = Convert.ToDecimal(Result["GrossBill"].ToString());
                    objCoreData.GrossTotal = Convert.ToDecimal(Result["GrossTotal"].ToString());
                    objCoreData.CFSTotal = Convert.ToDecimal(Result["CFSTotal"].ToString());


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objCoreData;
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

        #region Bill Cum SD Ledger
        public void GetBillCumSDLedgerReport(int partyId, string fromdate, string todate,string comname ,string address)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = partyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = fromdate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = todate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BillCumSDLedger", CommandType.StoredProcedure, DParam);
            BillCumSDLedger LedgerObj = new BillCumSDLedger();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {

                    LedgerObj.OpenningBalance = Convert.ToDecimal(Result["OpenningBalance"]);
                    LedgerObj.EximTraderName = Convert.ToString(Result["EximTraderName"]);
                    LedgerObj.EximTraderAlias = Convert.ToString(Result["EximTraderAlias"]);
                    
                    LedgerObj.COMGST = Convert.ToString(Result["COMGST"]);
                    LedgerObj.COMPAN = Convert.ToString(Result["COMPAN"]);
                    LedgerObj.CurDate = DateTime.Now.ToString("dd-MMM-yyyy");

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                       
                        LedgerObj.lstDetails.Add(new BillCumSDLedgerDetails
                        {
                            Sr = Convert.ToInt32(Result["Sr"]),
                            Col1 = Convert.ToString(Result["Col1"]),
                            Col2 = Convert.ToString(Result["Col2"]),
                            Col3 = Convert.ToString(Result["Col3"]),
                            Col4 = Convert.ToString(Result["Col4"]),
                            Col5 = Convert.ToString(Result["Col5"]),
                            Col6 = Convert.ToString(Result["Col6"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        LedgerObj.ClosingBalance= Convert.ToDecimal(Result["ClosingBalance"]);
                    }
                }
                

                if (Status == 1)
                {
                    LedgerObj.CompanyName = comname;
                    LedgerObj.CompanyAddress = address;
                    

                    /*
                    CrInvLedgerObj.ClosingBalance = (CrInvLedgerObj.OpenningBalance + (CrInvLedgerObj.lstLedgerSummary.Sum(o => o.Debit)))
                                                    - (CrInvLedgerObj.lstLedgerSummary.Sum(o => o.Credit));
                    */

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LedgerObj;
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

        #region ContainerArrivalReport
        public void ContainerArrivalReport(ContainerArrivalReport ObjContainerArrivalReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjContainerArrivalReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjContainerArrivalReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerArrivalReport.ImportExport });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EmptyLoaded", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerArrivalReport.EmptyLoaded });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ContainerArrivalReport", CommandType.StoredProcedure, DParam);
            //IList<ContainerArrivalReport> LstContainerArrivalReport = new List<ContainerArrivalReport>();
            ContainerArrivalReport LstContainerArrivalReport = new ContainerArrivalReport();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            int SizeTwenty = 0;
            int SizeFourty = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)

                    LstContainerArrivalReport.ListArrivalReport.Add(new ArrivalReportList
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        LoadOrEmpty = Result["LoadOrEmpty"].ToString(),
                        SealNo = Result["CustomSealNo"].ToString(),
                        Date = Result["Date"].ToString(),
                        Time = Result["Time"].ToString(),
                        Commodity = Result["Commodity"].ToString(),
                        ImportExport = Result["OperationType"].ToString(),
                        Size = Convert.ToString(Result["Size"] == null ? "" : Result["Size"]),
                        ICDCode = Result["ICDCode"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ModeOfTransport = Result["TransportMode"].ToString(),
                        // Withdraw = Result["Withdraw"].ToString()

                    });
                }

                if (Status == 1)
                {
                    SizeTwenty = LstContainerArrivalReport.ListArrivalReport.Count(o => o.Size == "20");
                    SizeFourty = LstContainerArrivalReport.ListArrivalReport.Count(o => o.Size == "40");
                    LstContainerArrivalReport.SizeTewnty = Convert.ToString(SizeTwenty);
                    LstContainerArrivalReport.SizeFourty = Convert.ToString(SizeFourty);
                     _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainerArrivalReport;
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
        #region ContainerOutReport
        public void ContainerOutport(ContainerOutReport ObjContainerOutReport)
        {
            int sizeTwenty = 0;
            int sizeFourty = 0;
            DateTime dtfrom = DateTime.ParseExact(ObjContainerOutReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjContainerOutReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerOutReport.ImportExport });
             LstParam.Add(new MySqlParameter { ParameterName = "in_LoadedEmpty", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerOutReport.LoadedEmpty });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ContainerOutReport", CommandType.StoredProcedure, DParam);
            ContainerOutReport LstContainerOutReport = new ContainerOutReport();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstContainerOutReport.lstContainerOutReport.Add(new ContainerOutReportList
                    {



                        Date = Result["Date"].ToString(),

                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        Time = Result["Time"].ToString(),
                        LoadedEmpty = Result["LoadedEmpty"].ToString(),
                        ImportExport = Result["ImportExport"].ToString()
                    });
                }

                if (Status == 1)
                {

                    if (LstContainerOutReport.lstContainerOutReport.Count > 0)
                    {
                        sizeTwenty = LstContainerOutReport.lstContainerOutReport.Count(o => o.Size == "20");
                        sizeFourty = LstContainerOutReport.lstContainerOutReport.Count(o => o.Size == "40");
                        //sizeTwenty = 0;
                        //sizeFourty = 0;

                        //LstContainerOutReport.lstContainerOutReport.ToList().ForEach(m =>
                        //{
                        //    if (m.Size == "20" && m.Size != null && m.Size != "")
                        //    {
                        //        sizeTwenty += 1;
                        //    }
                        //    if (m.Size == "40" && m.Size != null && m.Size != "")
                        //    {
                        //        sizeFourty += 1;
                        //    }

                        //});
                    }

                    LstContainerOutReport.SizeTwenty = Convert.ToString(sizeTwenty);
                    LstContainerOutReport.SizeFouirty = Convert.ToString(sizeFourty);                 

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainerOutReport;
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

        #region Container Payment Details
        public void ListOfContainerWithCFSCode(string CFSCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerWithCFSCode", CommandType.StoredProcedure, Dparam);
            IList<Ppg_ContainerList> lstContainer = new List<Ppg_ContainerList>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstContainer.Add(new Ppg_ContainerList
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"])
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
                    _DBResponse.Data = new { lstContainer, State };
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
        public void ContainerPaymentDetail(Ppg_ContainerPaymentInfo Obj)
        {
            int Status = 0;
            
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = Obj.CFSCode });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getContainerEnquiry", CommandType.StoredProcedure, DParam);
            //List<Ppg_ContainerPaymentDtl> LstMonthlyCashBook = new List<Ppg_ContainerPaymentDtl>();
            Ppg_ContainerPaymentInfo CPI = new Ppg_ContainerPaymentInfo();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    CPI.CFSCode = Convert.ToString(Result["CFSCode"]);
                    CPI.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    CPI.InDate = Convert.ToString(Result["EntryDateTime"]);
                    CPI.Size = Convert.ToString(Result["Size"]);
                    CPI.EximTraderAlias = Convert.ToString(Result["EximTraderAlias"]);
                    CPI.OutDate = Convert.ToString(Result["GateExitDateTime"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        CPI.lstContainerPaymentDtl.Add(new Ppg_ContainerPaymentDtl
                        {
                            ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),
                            ReceiptNo = Result["ReceiptNo"].ToString(),
                            PartyName = Result["PartyName"].ToString(),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            FromDate = Result["FromDate"].ToString(),
                            ToDate = Result["ToDate"].ToString(),
                            InvoiceType = Result["InvoiceType"].ToString()
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CPI;
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

        #region tds report
        public void TdsReport(TDSReport ObjTDSReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjTDSReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjTDSReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjTDSReport.PartyId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("TDSReport", CommandType.StoredProcedure, DParam);
            TDSMain objTDSMain = new TDSMain();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)

                    objTDSMain.TDSReportLst.Add(new TDSReport
                    {
                        CRNo = Result["ReceiptNo"].ToString(),
                        CRDate = Result["ReceiptDate"].ToString(),
                        PartyCode = Result["partycode"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        CertificateNo = Result["CertificateNo"].ToString(),
                        FinancialYear = Result["FINANCIALYEAR"].ToString(),
                        QUARTERMONTH = Result["QUARTERMONTH"].ToString(),
                        Amount = Result["Amount"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                    });
                }

                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        objTDSMain.TDSReportLst.Add(
                //        new TDSReport
                //        {
                //            CRNo = Result["ReceiptNo"].ToString(),
                //            CRDate = Result["ReceiptDate"].ToString(),
                //            PartyCode = Result["partycode"].ToString(),
                //            PartyName = Result["PartyName"].ToString(),
                //            CertificateNo = Result["CertificateNo"].ToString(),
                //            FinancialYear = Result["FINANCIALYEAR"].ToString(),
                //            QUARTERMONTH = Result["QUARTERMONTH"].ToString(),
                //            Amount = Result["Amount"].ToString(),
                //        });

                //objTDSMain.ObjTDSReporPartyWise.Party = Convert.ToString(Result["Party"]);
                //objTDSMain.ObjTDSReporPartyWise.Tan = Convert.ToString(Result["Tan"]);
                //objTDSMain.ObjTDSReporPartyWise.Value = Convert.ToString(Result["Value"]);
                //objTDSMain.ObjTDSReporPartyWise.TDS = Convert.ToString(Result["TDS"]);



                //    }
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objTDSMain;
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
        public void GetEximTraderForPartyLedger()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEximTraderForPartyLedger", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PartyLedgerList> LstParty = new List<PartyLedgerList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstParty.Add(new PartyLedgerList
                    {
                        PartyId = Convert.ToInt32(Result["EximTraderId"].ToString()),
                        PartyName = Result["EximTraderName"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstParty;
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

        #region Import Con Income
        public void ImportConIncomeDetail(Ppg_ImportConIncome Obj)
        {
            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.ToDate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getImportContainerIncome", CommandType.StoredProcedure, DParam);
            List<Ppg_ImportConIncomeDtl> lstImportConIncomeDtl = new List<Ppg_ImportConIncomeDtl>();
            
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new Ppg_ImportConIncomeDtl
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Result["PartyName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        ContainerNo = Result["PartyName"].ToString(),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        EntryNo = Result["GateInNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        MovementType = Result["MovementType"].ToString(),
                        TrainNo = Result["TrainNo"].ToString(),
                        TrainDate = Result["TrainDate"].ToString(),
                        SLACode = Convert.ToString(Result["SLACode"]),
                        PortCode = Result["PortCode"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        CustomSealNo = Result["CustomSealNo"].ToString(),
                        ShedNo = Result["ShedNo"].ToString(),
                        THC = Convert.ToDecimal(Result["THC"]),
                        TPT = Convert.ToDecimal(Result["TPT"]),
                        ECC = Convert.ToDecimal(Result["ECC"]),
                        DTF = Convert.ToDecimal(Result["DTF"]),
                        LOL = Convert.ToDecimal(Result["LOL"]),
                        IRR = Convert.ToDecimal(Result["IRR"]),
                        HAZ = Convert.ToDecimal(Result["HAZ"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstImportConIncomeDtl;
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

        #region Export Con Income
        public void ExportConIncomeDetail(Ppg_ExportConIncome Obj)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.ToDate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getExportContainerIncome", CommandType.StoredProcedure, DParam);
            List<Ppg_ExportConIncomeDtl> lstExportConIncomeDtl = new List<Ppg_ExportConIncomeDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExportConIncomeDtl.Add(new Ppg_ExportConIncomeDtl
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Result["PartyName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerType = Result["ContainerType"].ToString(),
                        Size = Result["Size"].ToString(),
                        MovementType = Result["MovementType"].ToString(),
                        CargoWeight = Convert.ToDecimal(Result["CargoWeight"]),
                        TareWeight = Convert.ToDecimal(Result["TareWeight"]),
                        TotalWeight = Convert.ToDecimal(Result["TotalWeight"]),
                        SLACode = Convert.ToString(Result["SLACode"]),
                        PortCode = Result["PortCode"].ToString(),
                        PortOfLoading = Result["PortOfLoading"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        MovementDate = Result["MovementDate"].ToString(),
                        SealDate = Result["SealDate"].ToString(),
                        ShedNo = Result["ShedNo"].ToString(),
                        ShedArea = Result["ShedArea"].ToString(),
                        THC = Convert.ToDecimal(Result["THC"]),
                        HND = Convert.ToDecimal(Result["HND"]),
                        RR = Convert.ToDecimal(Result["RR"]),
                        FNC = Convert.ToDecimal(Result["FNC"]),
                        WHT = Convert.ToDecimal(Result["WHT"]),
                        GRL = Convert.ToDecimal(Result["GRL"]),
                        GRE = Convert.ToDecimal(Result["GRE"]),
                        MO = Convert.ToDecimal(Result["MO"]),
                        INS = Convert.ToDecimal(Result["INS"]),
                        GEN = Convert.ToDecimal(Result["GEN"]),
                        HAZ = Convert.ToDecimal(Result["HAZ"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstExportConIncomeDtl;
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

        #region Assessment sheet LCL Report
        public void AssessmentSheetLCLDetail(Ppg_AssessmentSheetLCL Obj)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getAssessmentSheetLCL", CommandType.StoredProcedure, DParam);
            List<Ppg_AssessmentSheetLCLDtl> lstImportConIncomeDtl = new List<Ppg_AssessmentSheetLCLDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new Ppg_AssessmentSheetLCLDtl
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        ImporterName = Result["ImporterName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        PayeeCode = Result["PayeeCode"].ToString(),
                        BOENo = Result["BOENo"].ToString(),
                        BOEValueDuty = Convert.ToDecimal(Result["BOEValueDuty"]),
                        Area = Convert.ToDecimal(Result["Area"]),
                        GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                        Week = Convert.ToInt32(Result["Week"]),
                        CargoType = Result["CargoType"].ToString(),
                        ENT = Convert.ToDecimal(Result["ENT"]),
                        HND = Convert.ToDecimal(Result["HND"]),
                        STO = Convert.ToDecimal(Result["STO"]),
                        INS = Convert.ToDecimal(Result["INS"]),
                        OTI = Convert.ToDecimal(Result["OTI"]),
                        HAZ = Convert.ToDecimal(Result["HAZ"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstImportConIncomeDtl;
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

        #region Seal Closing Report
        public void SealClosingReport(Ppg_SealClosingReport ObjSealClosingReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjSealClosingReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjSealClosingReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("SealClosingReport", CommandType.StoredProcedure, DParam);
            IList<Ppg_SealClosingReport> LstSealClosingReport = new List<Ppg_SealClosingReport>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)

                    LstSealClosingReport.Add(new Ppg_SealClosingReport
                    {


                        Date = Result["Date"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        Commodity = Result["Commodity"].ToString(),
                        LCLFCL = Result["LCLFCL"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        CHAOrPort = Result["CHA"].ToString(),
                        Weight = Result["StuffWeight"].ToString(),

                        value = Result["value"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSealClosingReport;
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


        #region Empty Container Payment Report
        public void EmptyContainerPayment(Ppg_EmptyConPayRpt Obj)
        {
            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getEmptyContainerPaymentReport", CommandType.StoredProcedure, DParam);
            List<Ppg_EmptyConPayRptDtl> lstEmptyContDtl = new List<Ppg_EmptyConPayRptDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstEmptyContDtl.Add(new Ppg_EmptyConPayRptDtl
                    {
                        InvoiceId=Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        ImporterName = Result["ImporterName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        TotalDays= Convert.ToInt32(Result["TotalDays"]),
                        LOE = Convert.ToDecimal(Result["LOE"]),
                        GRE = Convert.ToDecimal(Result["GRE"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstEmptyContDtl;
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

        #region Reserve Space Income Report
        public void ReserveSpaceIncomeReport(DateTime FromDate,DateTime ToDate)
        {
           
            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = ToDate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReserveSpaceIncomeReport", CommandType.StoredProcedure, DParam);
            List<ReserveSpaceReport> lstEmptyContDtl = new List<ReserveSpaceReport>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstEmptyContDtl.Add(new ReserveSpaceReport
                    {
                       
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                         InvoiceDate = Result["InvoiceDate"].ToString(),
                         //SLNo =Convert.ToInt32(Result["SlNo"]),
                         TotalAMount = Convert.ToDecimal(Result["InvoiceAmt"].ToString()),
                         SGST = Convert.ToDecimal(Result["TotalSGST"].ToString()),
                         ReservationAmount = Convert.ToDecimal(Result["TotalTaxable"].ToString()),
                         Remarks = Result["Remarks"].ToString(),
                         Rate = Convert.ToDecimal(Result["Rent"].ToString()),                        
                         PartyName = Result["PartyName"].ToString(),
                         PartyCode = Result["PartyCode"].ToString(),
                         Month = Result["Month"].ToString(),
                         AmountReceivable = Convert.ToDecimal(Result["OutstandingAmt"].ToString()),
                         Area = Convert.ToDecimal(Result["Area"].ToString()),
                         BillingDate ="", //Result["InvoiceDate"].ToString(),
                         BillingNo = "",//Result["InvoiceNo"].ToString(),
                         CGST = Convert.ToDecimal(Result["TotalCGST"].ToString()),
                         IGST = Convert.ToDecimal(Result["TotalIGST"].ToString())

                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstEmptyContDtl;
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

        #region Outstanding Amount Report
        public void OutstandingAmountReport(DateTime FromDate, DateTime ToDate)
        {

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = ToDate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GETOutstandingAmountReport", CommandType.StoredProcedure, DParam);
            List<Ppg_OutstandingAmountReport> lstEmptyContDtl = new List<Ppg_OutstandingAmountReport>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstEmptyContDtl.Add(new Ppg_OutstandingAmountReport
                    {

                        BillingNo = Result["InvoiceNo"].ToString(),
                        BillingDate = Result["InvoiceDate"].ToString(),
                        //SLNo =Convert.ToInt32(Result["SlNo"]),
                        AmountReceivalbe= Convert.ToDecimal(Result["GrossTotal"].ToString()),
                        TotalAmount = Convert.ToDecimal(Result["Amount"].ToString()),
                        SGST = Convert.ToDecimal(Result["SGSTAmt"].ToString()),                       
                        Remarks = Result["Remarks"].ToString(),
                        SQM = Convert.ToDecimal(Result["Rate"].ToString()),
                        PartyName = Result["PartyName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        Month = Result["Month"].ToString(),                       
                        Area = Convert.ToDecimal(Result["Area"].ToString()),                      
                        CGST = Convert.ToDecimal(Result["CGSTAmt"].ToString()),
                        IGST = Convert.ToDecimal(Result["IGSTAmt"].ToString())

                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstEmptyContDtl;
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


        #region Assessment sheet FCL Report
        public void AssessmentSheetFCLDetail(Ppg_AssessmentSheetFCL Obj)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAssessmentSheetFCL", CommandType.StoredProcedure, DParam);
            List<Ppg_AssessmentSheetFCLDtl> lstImportConIncomeDtl = new List<Ppg_AssessmentSheetFCLDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new Ppg_AssessmentSheetFCLDtl
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        ImporterName = Result["ImporterName"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        //PayeeCode = Result["PayeeCode"].ToString(),
                        BOENo = Result["BOENo"].ToString(),
                        BOEValueDuty = Convert.ToDecimal(Result["BEOValue"]),
                        //Area = Convert.ToDecimal(Result["Area"]),
                        GrossWt = Convert.ToDecimal(Result["TotalGrossWt"]),
                        Days = Convert.ToInt32(Result["Days"]),
                        CargoType = Result["CargoType"].ToString(),
                        ENT = Convert.ToDecimal(Result["ENT"]),
                        GRL = Convert.ToDecimal(Result["GRL"]),
                        MF = Convert.ToDecimal(Result["MF"]),
                        INS = Convert.ToDecimal(Result["INS"]),
                        OTI = Convert.ToDecimal(Result["OTI"]),
                        HAZ = Convert.ToDecimal(Result["HAZ"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstImportConIncomeDtl;
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


        #region Rent Income Report
        public void RentIncomeReport(Ppg_RentIncomeReportViewModel Obj)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetRentIncomeReport", CommandType.StoredProcedure, DParam);
            List<Ppg_RentIncomeReportViewModel> lstImportConIncomeDtl = new List<Ppg_RentIncomeReportViewModel>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new Ppg_RentIncomeReportViewModel
                    {
                        //i = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Result["EximTraderName"].ToString(),
                        PartyCode= Result["EximTraderAlias"].ToString(),
                         Month= Result["Month"].ToString(),
                         TDSCertification= Result["CirtificateNo"].ToString(),
                         RentReceived=Convert.ToDecimal(Result["Taxable"]) ,
                         TDSAmount= Convert.ToDecimal(Result["TDSAmount"]),
                         SGST= Convert.ToDecimal(Result["SGSTAmt"]),
                         CGST= Convert.ToDecimal(Result["CGSTAmt"]),
                         IGST= Convert.ToDecimal(Result["IGSTAmt"]),
                         TotalAmountReceived= Convert.ToDecimal(Result["Total"]),
                         AmountOutstanding = (Convert.ToDecimal(Result["Total"])- Convert.ToDecimal(Result["ReceivedAmount"])),
                         Remarks= Result["Remarks"].ToString()

                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstImportConIncomeDtl;
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


        #region Cargo Stock Register
        public void CargoStockRegister(Ppg_CrgStkRgt ObjCargoStockRegister)
        {
            DateTime dtfrom = DateTime.ParseExact(ObjCargoStockRegister.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjCargoStockRegister.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("CargoStockRegister", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Ppg_CrgStkRgt _ObjCargoStockRegister = new Ppg_CrgStkRgt();
            IList<ppgexportCargoStock> ppgexportCargoStocklst = new List<ppgexportCargoStock>();

            IList<ppgimportCargoStock> ppgimportCargoStocklst = new List<ppgimportCargoStock>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    _ObjCargoStockRegister.ppgexportCargoStocklst.Add(new ppgexportCargoStock
                    {
                        shippingBillNo = Result["ShippingBillNo"].ToString().Trim(),
                        Date = (Result["RegisterDate"] == null ? "" : Result["RegisterDate"]).ToString(),
                        NoOfPackage = (Result["NoOfUnits"] == null ? "" : Result["NoOfUnits"]).ToString(),
                        Commodity = (Result["CommodityName"] == null ? "" : Result["CommodityName"]).ToString(),
                        Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString(),

                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        _ObjCargoStockRegister.ppgimportCargoStocklst.Add(new ppgimportCargoStock
                        {
                            BOE = Result["BOENo"].ToString(),
                            Date = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            NoOfPackage = (Result["NoOfUnits"] == null ? "" : Result["NoOfUnits"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString(),

                        });
                    }



                }

                /*  if (Result.NextResult())
                  {
                      while (Result.Read())
                      {
                          Status = 1;
                          _ObjCargoStockRegister.bondCargoStocklst.Add(new bondCargoStock
                          {
                              Warehouse = Result["WRNo"].ToString(),
                              Date = (Result["WRDate"] == null ? "" : Result["WRDate"]).ToString(),
                              NoOfPackage = (Result["NoOfUnits"] == null ? "" : Result["NoOfUnits"]).ToString(),
                              Commodity = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                              Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString(),

                          });
                      }



                  }*/
                if (Status == 1)
                {
                    _ObjCargoStockRegister.ppgexportCargoStocklst.ToList().ForEach(m => {
                        if (m.shippingBillNo == "/")
                        {
                            m.shippingBillNo = "";
                        }

                    });

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = _ObjCargoStockRegister;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
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


        #region Tax (0) Invoice Report
        public void TaxZeroInvoiceReport(DailyCashBookPpg ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("TaxZeroInvoiceReport", CommandType.StoredProcedure, DParam);
            IList<DailyCashBookPpg> LstDailyCashBook = new List<DailyCashBookPpg>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)

                    LstDailyCashBook.Add(new DailyCashBookPpg
                    {

                        //ReceiptNo, ReceiptDate, Party, ChequeNo, CashReceiptId, GenSpace, sto, Insurance, GroundRentEmpty, GroundRentLoaded, Mf, EntCharge, 
                        //Fum, OtCharge, CGSTAmt, SGSTAmt, IGSTAmt, MISC, MiscExcess, TotalCash, TotalCheque, tdsCol, crTDS
                        /*CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),

                        Depositor = Result["Party"].ToString(),*/
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        InvoiceType = Result["InvoiceType"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        ModeOfPay = Result["ModeOfPay"].ToString(),

                        ChqNo = Result["ChequeNo"].ToString(),
                        GenSpace = Result["GenSpace"].ToString(),
                        StorageCharge = Result["sto"].ToString(),
                        Insurance = Result["Insurance"].ToString(),
                        GroundRentEmpty = Result["GroundRentEmpty"].ToString(),
                        GroundRentLoaded = Result["GroundRentLoaded"].ToString(),
                        MfCharge = Result["Mf"].ToString(),
                        EntryCharge = Result["EntCharge"].ToString(),
                        Fumigation = Result["Fum"].ToString(),
                        OtherCharge = Result["OtCharge"].ToString(),
                        Misc = Result["MISC"].ToString(),
                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),

                        MiscExcess = Result["MiscExcess"].ToString(),
                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        TotalOthers = Result["TotalOther"].ToString(),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString(),
                        TotalPDA = Result["TotalPDA"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        //TDSPlus = Result["TDSPlus"].ToString(),
                        //Exempted = Result["Exempted"].ToString(),
                        //PdaPLus = Result["PdaPLus"].ToString(),
                        //TDSMinus = Result["TDSMinus"].ToString(),
                        //PdaMinus = Result["PdaMinus"].ToString(),
                        //HtAdjust = Result["HtAdjust"].ToString(),
                        //RoundOff = Result["RoundUp"].ToString(),
                        //RowTotal = Result["Total"].ToString()


                        //Party = Result["Party"].ToString(),
                        //Deposit = Result["Deposit"].ToString(),
                        //Withdraw = Result["Withdraw"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDailyCashBook;
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

        #region PDUtilizationSummary
        public void PdSummaryUtilizationReport(PdSummary ObjPdSummaryReport, int type = 1)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjPdSummaryReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            // DateTime dtTo = DateTime.ParseExact(ObjCargoInStockReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_asOndate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.Int32, Value = type });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PdSummaryUtilizationReport", CommandType.StoredProcedure, DParam);
            IList<PdSummary> LstPdSummaryReport = new List<PdSummary>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)

                    LstPdSummaryReport.Add(new PdSummary
                    {



                        PartyName = Result["PartyName"].ToString(),

                        Amount = Result["Amount"].ToString()

                        //ContainerNo = Result["ContainerNo"].ToString(),
                        //value = Result["value"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPdSummaryReport;
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

        #region Export THC RR
        public void ExportTHCRRReport(Ppg_ThcRrReport vm)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(vm.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(vm.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetTHCRRReport", CommandType.StoredProcedure, DParam);
            List<Ppg_ThcRrReport> lstExportTHCRRdtl = new List<Ppg_ThcRrReport>();

            _DBResponse = new DatabaseResponse();
            try
            {
             
                while (Result.Read())
                {
                    Status = 1;

                    lstExportTHCRRdtl.Add(new Ppg_ThcRrReport
                    {
                       
                       Date=Convert.ToString(Result["InvoiceDate"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLinaName"]),
                        PayeeCode = Convert.ToString(Result["EximTraderAlias"]),
                        DestinationPort = Convert.ToString(Result["PortName"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerSize = Convert.ToString(Result["Size"]),
                        TotalWeight = Convert.ToDecimal(Result["TotalGrossWt"]),
                        TareWeight = Convert.ToDecimal(Result["TareWeight"]),
                        RRAmount = Convert.ToDecimal(Result["RRAmount"]),
                        TotalRRGstAmount = Convert.ToDecimal(Result["RRTotalGSTAmt"]),
                        THCAmount = Convert.ToDecimal(Result["THCAmount"]),
                        TotalTHGstAmount = Convert.ToDecimal(Result["THCTotalGSTAmt"]),
                        GrandTotal = Convert.ToDecimal(Result["Total"])
                    });
                   
                }


                lstExportTHCRRdtl.Add(new Ppg_ThcRrReport
                {

                    Date = "Total",
                    InvoiceNo = "",
                    ShippingLineName = "",
                    PayeeCode = "",
                    DestinationPort = "",
                    ContainerNo = "",
                    CFSCode = "",
                    ContainerSize="",
                    TotalWeight=lstExportTHCRRdtl.Sum(x=>x.TotalWeight),
                    TareWeight = lstExportTHCRRdtl.Sum(x => x.TareWeight),
                    RRAmount = lstExportTHCRRdtl.Sum(x => x.RRAmount),
                    TotalRRGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalRRGstAmount),
                    THCAmount = lstExportTHCRRdtl.Sum(x => x.THCAmount),
                    TotalTHGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalTHGstAmount),
                    GrandTotal = lstExportTHCRRdtl.Sum(x => x.GrandTotal)                  

                });
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstExportTHCRRdtl;
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


        #region Import THC RR
        public void ImportTHCRRReport(Ppg_ThcRrReport vm)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(vm.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(vm.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImportTHCRRReport", CommandType.StoredProcedure, DParam);
            List<Ppg_ThcRrReport> lstExportTHCRRdtl = new List<Ppg_ThcRrReport>();

            _DBResponse = new DatabaseResponse();
            try
            {

                while (Result.Read())
                {
                    Status = 1;

                    lstExportTHCRRdtl.Add(new Ppg_ThcRrReport
                    {

                        Date = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLinaName"]),
                        PayeeCode = Convert.ToString(Result["EximTraderAlias"]),
                        DestinationPort = Convert.ToString(Result["PortName"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerSize = Convert.ToString(Result["Size"]),
                        TrainNo = Convert.ToString(Result["TrainNo"]),
                        TrainDate = Convert.ToString(Result["TrainDate"]),
                        TotalWeight = Convert.ToDecimal(Result["Gross_Wt"]),
                        TareWeight = Convert.ToDecimal(Result["Ct_Tare"]),
                        RRAmount = Convert.ToDecimal(Result["RRAmount"]),
                        TotalRRGstAmount = Convert.ToDecimal(Result["RRTotalGSTAmt"]),
                        THCAmount = Convert.ToDecimal(Result["THCAmount"]),
                        TotalTHGstAmount = Convert.ToDecimal(Result["THCTotalGSTAmt"]),
                        GrandTotal = Convert.ToDecimal(Result["Total"])
                    });

                }


                lstExportTHCRRdtl.Add(new Ppg_ThcRrReport
                {

                    Date = "Total",
                    InvoiceNo = "",
                    ShippingLineName = "",
                    PayeeCode = "",
                    DestinationPort = "",
                    ContainerNo = "",
                    CFSCode = "",
                    ContainerSize = "",
                    TrainNo = "",
                    TrainDate="",
                    TareWeight = lstExportTHCRRdtl.Sum(x => x.TareWeight),
                    TotalWeight = lstExportTHCRRdtl.Sum(x => x.TotalWeight),
                    RRAmount = lstExportTHCRRdtl.Sum(x => x.RRAmount),
                    TotalRRGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalRRGstAmount),
                    THCAmount = lstExportTHCRRdtl.Sum(x => x.THCAmount),
                    TotalTHGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalTHGstAmount),
                    GrandTotal = lstExportTHCRRdtl.Sum(x => x.GrandTotal)

                });
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstExportTHCRRdtl;
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

        #region Time Barred Report

        public void GetTimeBarredReport(string AsOnDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd") });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Module });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("TimeBarredReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Wlj_TimeBarredReport> lstPV = new List<Wlj_TimeBarredReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Wlj_TimeBarredReport
                    {
                        BondNo = Result["BondNo"].ToString(),
                        BondDate = Result["BondDate"].ToString(),
                        Importer = Result["Importer"].ToString(),
                        ItemDesc = Result["ItemDesc"].ToString(),
                        Units = Convert.ToInt32(Result["Unit"]),
                        Value = Convert.ToDecimal(Result["Value"]),
                        Duty = Convert.ToDecimal(Result["Duty"]),
                        Area = Convert.ToDecimal(Result["Area"]),
                        CHA = Result["CHA"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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

        #region Live Bond Report
        public void GetLiveBondReport(string AsOnDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd") });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Module });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("LiveBondReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Wlj_TimeBarredReport> lstPV = new List<Wlj_TimeBarredReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Wlj_TimeBarredReport
                    {
                        BondNo = Result["BondNo"].ToString(),
                        BondDate = Result["BondDate"].ToString(),
                        Importer = Result["Importer"].ToString(),
                        ItemDesc = Result["ItemDesc"].ToString(),
                        Units = Convert.ToInt32(Result["Unit"]),
                        Value = Convert.ToDecimal(Result["Value"]),
                        Duty = Convert.ToDecimal(Result["Duty"]),
                        Area = Convert.ToDecimal(Result["Area"]),
                        CHA = Result["CHA"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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

        #region-- VC REPORT --
      
        public void VCCapacityDetails(string dtArray, string date1, string date2)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_Xml", MySqlDbType = MySqlDbType.Text, Value = dtArray });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(date1).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(date2).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //var Result1 = DataAccess.ExecuteDynamicSet<VCCapacityModel>("GetVCCapacityDetails", DParam);
            IDataReader Result = DataAccess.ExecuteDataReader("GetVCCapDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<WljVCCapacity> LstVCCapacity = new List<WljVCCapacity>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstVCCapacity.Add(new WljVCCapacity
                    {
                        //vccapid = (Result["vccapid"] == DBNull.Value ? 0 : Convert.ToInt32(Result["vccapid"])),
                        //vcoptdate = (Result["vcoptdate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(Result["vcoptdate"])),
                        cfscapopen = (Result["cfscapopen"] == DBNull.Value ? 496034M : Convert.ToDecimal(Result["cfscapopen"])),
                        cfscapclose = (Result["cfscapclose"] == DBNull.Value ? 8766M : Convert.ToDecimal(Result["cfscapclose"])),
                        bndcapclose = (Result["bndcapclose"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["bndcapclose"])),
                        totcov = (Result["totcov"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["totcov"])),
                        cfsopenutlz = (Result["cfsopenutlz"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["cfsopenutlz"])),
                        cfscloseutlz = (Result["cfscloseutlz"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["cfscloseutlz"])),
                        cfscloseutlzOrg = (Result["cfscloseutlzorg"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["cfscloseutlzorg"])),

                        bndcloseutlz = (Result["bndcloseutlz"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["bndcloseutlz"])),
                        bndcloseutlzOrg = (Result["bndcloseutlzorg"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["bndcloseutlzorg"])),

                        totutil = (Result["totutil"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["totutil"]))
                        //weekid = Result["weekid"].ToString()
                    });
                }
                if (LstVCCapacity.Count() < 3)
                {
                    for (var i = LstVCCapacity.Count; i < 3; i++)
                    {
                        LstVCCapacity.Add(new WljVCCapacity());
                    }
                }
                if (Result.NextResult())
                {
                    var row = 0;
                    while (Result.Read())
                    {
                        LstVCCapacity[row].Occupency = (Result["occupency"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["occupency"]));
                        LstVCCapacity[row].Income = (Result["income"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["income"]));
                        LstVCCapacity[row].ContHand = (Result["ContHand"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["ContHand"]));
                        LstVCCapacity[row].Bond = (Result["Bond"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["Bond"]));
                        LstVCCapacity[row].Doc = (Result["Doc"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["Doc"]));
                        LstVCCapacity[row].Egm = (Result["EGM"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["EGM"]));
                        row += 1;
                    }
                }
                if (Result.NextResult())
                {
                    var row = 0;
                    while (Result.Read())
                    {
                        LstVCCapacity[row].Import = (Result["Import"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Import"]));
                        LstVCCapacity[row].Export = (Result["Export"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Export"]));
                        LstVCCapacity[row].Empty = (Result["Empty"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Empty"]));
                        row += 1;
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstVCCapacity;
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
        public void AddVCDetails(DateTime date, decimal cfscapopen, decimal cfscapcovered, decimal bndcapcovered, decimal total1, decimal cfsutilopen, decimal cfsutilcover, decimal bndutilcover, decimal total2)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_Date", MySqlDbType = MySqlDbType.DateTime, Value = date });
            LstParam.Add(new MySqlParameter { ParameterName = "In_cfscapopen", MySqlDbType = MySqlDbType.Decimal, Value = cfscapopen });
            LstParam.Add(new MySqlParameter { ParameterName = "In_cfscapcovered", MySqlDbType = MySqlDbType.Decimal, Value = cfscapcovered });
            LstParam.Add(new MySqlParameter { ParameterName = "In_bndcapcovered", MySqlDbType = MySqlDbType.Decimal, Value = bndcapcovered });
            LstParam.Add(new MySqlParameter { ParameterName = "In_total1", MySqlDbType = MySqlDbType.Decimal, Value = total1 });
            LstParam.Add(new MySqlParameter { ParameterName = "In_cfsutilopen", MySqlDbType = MySqlDbType.Decimal, Value = cfsutilopen });
            LstParam.Add(new MySqlParameter { ParameterName = "In_cfsutilcover", MySqlDbType = MySqlDbType.Decimal, Value = cfsutilcover });
            LstParam.Add(new MySqlParameter { ParameterName = "In_bndutilcover", MySqlDbType = MySqlDbType.Decimal, Value = bndutilcover });
            LstParam.Add(new MySqlParameter { ParameterName = "In_total2", MySqlDbType = MySqlDbType.Decimal, Value = total2 });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            _DBResponse = new DatabaseResponse();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddVCCAPDetails", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Bank Details Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Bank And Branch Combination Already Exist";
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


        #region ECONOMY REPORT
        public void GetEconomyReport(int month, int year)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "repoYear", MySqlDbType = MySqlDbType.VarChar, Value = year });
            LstParam.Add(new MySqlParameter { ParameterName = "repoMonth", MySqlDbType = MySqlDbType.Int32, Value = month });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMonthlyEconomyRpt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<EconomyReport> LstData = new List<EconomyReport>();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    LstData.Add(new EconomyReport
                    {
                        IncomeExpHeadId = Convert.ToInt32(Result["IncomeExpHeadId"]),
                        ItemType = Result["ItemType"].ToString(),
                        ItemHead = Result["ItemHead"].ToString(),
                        ItemLabel = Result["ItemLabel"].ToString(),
                        Amount = Result["Amount"].ToString(),
                        CumAmount = Result["CumAmount"].ToString(),
                        ProCumAmount = Result["ProCumAmount"].ToString(),
                        IsTextBox = Convert.ToInt32(Result["IsTextBox"])

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstData;
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

        public void SaveEconomyReport(int Month, int Year, string xmlMonthlyEconomyReport, int Uid)
        {
            string GenId = "0";
            string RetObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "repoYear", MySqlDbType = MySqlDbType.Int32, Value = Year });
            LstParam.Add(new MySqlParameter { ParameterName = "repoMonth", MySqlDbType = MySqlDbType.Int32, Value = Month });
            LstParam.Add(new MySqlParameter { ParameterName = "uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "xmlMonthlyEconomyReport", MySqlDbType = MySqlDbType.LongText, Value = xmlMonthlyEconomyReport });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GenId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.Text, Direction = ParameterDirection.Output, Value = RetObj });
            _DBResponse = new DatabaseResponse();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("RptMonthlyEconomySave", CommandType.StoredProcedure, DParam, out GenId, out RetObj);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = RetObj;
                    _DBResponse.Data = RetObj;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = RetObj;
                    _DBResponse.Data = RetObj;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = RetObj;
                    _DBResponse.Data = RetObj;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }


        public void GetEconomyReportForPrint(int month, int year)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "repoYear", MySqlDbType = MySqlDbType.VarChar, Value = year });
            LstParam.Add(new MySqlParameter { ParameterName = "repoMonth", MySqlDbType = MySqlDbType.Int32, Value = month });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMonthlyEconomyRptForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            EconomyRptPrint PrintData = new EconomyRptPrint();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    PrintData.FormDate = Result["FormDate"].ToString();
                    PrintData.ToDate = Result["ToDate"].ToString();
                    PrintData.SqmCovered = Result["SqmCovered"].ToString();
                    PrintData.BagCovered = Result["BagCovered"].ToString();
                    PrintData.SqmOpen = Result["SqmOpen"].ToString();
                    PrintData.BagOpen = Result["BagOpen"].ToString();
                    PrintData.CreatedOn = Result["CreatedOn"].ToString();
                    PrintData.IsFound = Convert.ToInt32(Result["IsFound"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        PrintData.RptDetails.Add(new EconomyReport
                        {
                            Sr = Convert.ToInt32(Result["Sr"]),
                            IncomeExpHeadId = Convert.ToInt32(Result["IncomeExpHeadId"]),
                            ItemType = Result["ItemType"].ToString(),
                            ItemHead = Result["ItemHead"].ToString(),
                            ItemLabel = Result["ItemLabel"].ToString(),
                            Amount = Result["Amount"].ToString(),
                            CumAmount = Result["CumAmount"].ToString(),
                            ProCumAmount = Result["ProCumAmount"].ToString(),
                            ItemCodeNo = Result["ItemCodeNo"].ToString(),
                            ItemSortOrder = Convert.ToInt32(Result["ItemSortOrder"]),
                            PageNo = Convert.ToInt32(Result["PageNo"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        PrintData.RptSummary.Add(new EconomyRptIncomeSummary
                        {
                            Sr = Convert.ToInt32(Result["Sr"]),
                            IncomeExpHeadId = Convert.ToInt32(Result["IncomeExpHeadId"]),
                            ItemLabel = Result["ItemLabel"].ToString(),
                            Amount = Result["Amount"].ToString(),
                            CumAmount = Result["CumAmount"].ToString(),
                            ProCumAmount = Result["ProCumAmount"].ToString(),
                            CodeNo = Result["CodeNo"].ToString(),

                        });
                    }
                }

                if (PrintData.IsFound == 0)
                {
                    Status = 0;
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = PrintData;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data saved for this month and year";
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

        #region SAC Register
        public void GetSACDetails(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("SACRegister", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Wlj_SACRegister> lstSAC = new List<Wlj_SACRegister>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstSAC.Add(new Wlj_SACRegister
                    {
                        SacNo = Result["SacNo"].ToString(),
                        SacDate = Result["SacDate"].ToString(),
                        ValidUpto = Result["ValidUpto"].ToString(),
                        IMPName = Result["IMPName"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        SealNo = Result["SealNo"].ToString(),
                        BOLAWBNo = Result["BOLAWBNo"].ToString(),
                        BOENo = Result["BOENo"].ToString(),
                        BOEDate = Result["BOEDate"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        NoOfUnits = Result["NoOfUnits"].ToString(),
                        AreaReserved = Result["AreaReserved"].ToString(),
                        InvoiceAmt = Result["InvoiceAmt"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Result["ReceiptDate"].ToString(),
                        Remarks = Result["Remarks"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstSAC;
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

        #region Bond Register
        public void GetBondNo()
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.Date, Value = DBNull.Value });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.Date, Value = DBNull.Value });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BondRegister", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<Wlj_BondDetails> lstBond = new List<Wlj_BondDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBond.Add(new Wlj_BondDetails
                    {
                        BondNo = Result["BondNo"].ToString(),
                        DepositAppId = Convert.ToInt32(Result["DepositAppId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBond;
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
        public void GetBondRegister(int DepositeAppId, string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = DepositeAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.Date, Value = (FromDate == "" ? null : Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd")) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.Date, Value = (ToDate == "" ? null : Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd")) });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BondRegister", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Wlj_BondRegister objBond = new Wlj_BondRegister();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objBond.lstSACDetails.Add(new Wlj_SACDetails
                    {
                        CompanyAdd = Convert.ToString(Result["CompanyAddress"]),
                        CompanyEmail = Convert.ToString(Result["CompananyEmail"]),
                        DepositappId = Convert.ToInt32(Result["DepositAppId"].ToString()),
                        IMPName = Result["IMPName"].ToString(),
                        IMPAdd = Result["IMPAdd"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        CHAdd = Result["CHAdd"].ToString(),
                        SacNo = Result["SacNo"].ToString(),
                        SacDate = Result["SacDate"].ToString(),
                        ValidUpto = Result["ValidUpto"].ToString(),
                        AreaReserved = Convert.ToDecimal(Result["AreaReserved"]),
                        GodownName = Result["GodownName"].ToString(),
                        BondNo = Result["BondNo"].ToString(),
                        BondDate = Result["BondDate"].ToString(),
                        BondBOENo = Result["BondBOENo"].ToString(),
                        BondBOEDate = Result["BondBOEDate"].ToString(),
                        ExpDateofWarehouse = Result["ExpDateofWarehouse"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objBond.lstAdvPayment.Add(new Wlj_SACAdvancePayment
                        {
                            SacNo = Result["StuffingReqNo"].ToString(),
                            InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]),
                            ReceiptNo = Result["ReceiptNo"].ToString(),
                            ReceiptDate = Result["ReceiptDate"].ToString(),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objBond.lstUnloadingDetails.Add(new Wlj_UnloadingDetails
                        {
                            DepositappId = Convert.ToInt32(Result["DepositAppId"].ToString()),
                            UnloadedDate = Convert.ToString(Result["UnloadedDate"]),
                            UnloadedUnits = Convert.ToInt32(Result["UnloadedUnits"]),
                            UnloadedWeights = Convert.ToDecimal(Result["UnloadedWeights"]),
                            AreaOccupied = Convert.ToDecimal(Result["AreaOccupied"]),
                            PackageCondition = Result["PackageCondition"].ToString(),
                            Value = Convert.ToDecimal(Result["Value"]),
                            Duty = Convert.ToDecimal(Result["Duty"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objBond.lstDeliveryDetails.Add(new Wlj_DeliveryDetails
                        {
                            DepositappId = Convert.ToInt32(Result["DepositAppId"].ToString()),
                            DeliveryOrderNo = Result["DeliveryOrderNo"].ToString(),
                            Units = Convert.ToInt32(Result["Units"]),
                            Weight = Convert.ToDecimal(Result["Weight"]),
                            SQM = Convert.ToDecimal(Result["SQM"]),
                            Value = Convert.ToDecimal(Result["Value"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            BondBOENo = Result["BondBOENo"].ToString(),
                            BondBOEDate = Result["BondBOEDate"].ToString(),
                            DeliveryOrderDtlId = Convert.ToInt32(Result["DeliveryOrderDtlId"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objBond.lstDeliveryPaymentDet.Add(new Wlj_DeliveryPayementDetails
                        {
                            DepositappId = Convert.ToInt32(Result["DepositAppId"].ToString()),
                            InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]),
                            DeliveryDate = Convert.ToString(Result["DeliveryDate"]),
                            ReceiptNo = Result["ReceiptNo"].ToString(),
                            ReceiptDate = Result["ReceiptDate"].ToString(),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objBond;
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

        
        #region GST and MIS Report


        public void GetGSTMISReport( string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();            
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = (FromDate == "" ? null : Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd")) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = (ToDate == "" ? null : Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd")) });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GSTMISReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Wlj_GSTMISReport objGSTMIS = new Wlj_GSTMISReport();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objGSTMIS.lstCash.Add(new Wlj_CASHPDReport
                    {
                        ChargeType = Convert.ToString(Result["ChargeType"]),
                        ChargeName = Convert.ToString(Result["ChargeName"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])                                             
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objGSTMIS.lstSD.Add(new Wlj_CASHPDReport
                        {

                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            CGST = Convert.ToDecimal(Result["CGST"]),
                            SGST = Convert.ToDecimal(Result["SGST"]),
                            IGST = Convert.ToDecimal(Result["IGST"]),
                            Total = Convert.ToDecimal(Result["Total"])
                        });
                    }
                } 
               
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objGSTMIS;
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

        #region Register of Outward Supply
        public void GetRegisterofOutwardSupply(DateTime date1, DateTime date2)
        {

            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetRegisterofOutward", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            List<Wlj_RegisterOfOutward> model = new List<Wlj_RegisterOfOutward>();
           
            try
            {
                if (ds.Tables.Count > 0)
                {
                    model = (from DataRow dr in dt.Rows
                             select new Wlj_RegisterOfOutward()
                             {
                                 SlNo = Convert.ToInt32(dr["SlNo"]),
                                 InvoiceType= dr["InvoiceType"].ToString(),
                                 GST = dr["GST"].ToString(),
                                 Name = dr["Name"].ToString(),
                                 Place = dr["Place"].ToString(),
                                 InvoiceNo = dr["InvoiceNo"].ToString(),
                                 InvoiceDate = dr["InvoiceDate"].ToString(),
                                 SACCode= dr["SACCode"].ToString(),
                                  description= dr["Description"].ToString(),
                                  Rate= dr["Rate"].ToString(),
                                 ServiceValue =Convert.ToDecimal(dr["ServiceValue"]),
                                 ITaxAmount = Convert.ToDecimal(dr["ITaxAmount"]),
                                 CTaxAmount = Convert.ToDecimal(dr["CTaxAmount"]),
                                STaxAmount = Convert.ToDecimal(dr["STaxAmount"]),
                                   Cess = Convert.ToDecimal(dr["Cess"]),
                                 RoundUp = Convert.ToDecimal(dr["RoundUp"]),
                                 Total = Convert.ToDecimal(dr["Total"])
                }).ToList();
                }
              
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = RegisterofOutwardSupplyExcel(model,date1);
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally 
            {
                ds.Dispose();
            }
        }
        private string RegisterofOutwardSupplyExcel(List<Wlj_RegisterOfOutward> model, DateTime date1)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var date = date1.ToString();
            var PeriodFromMonth = date.Split('/')[1];
           

            var printMonth = "";
            var printYear = "";

           
                switch (PeriodFromMonth)
                {
                    //Here ds is list of invoice of a module between two dates 
                    case "01":
                        printMonth = "JANUARY";
                     
                        break;
                    case "02":
                        printMonth = "FEBRUARY";
                       
                        break;
                    case "03":
                        printMonth = "MARCH";
                       
                        break;
                    case "04":
                        printMonth = "APRIL";
                    
                        break;
                    case "05":
                        printMonth = "MAY";
                      
                        break;
                    case "06":
                        printMonth = "JUNE";
                      
                        break;
                    case "07":
                        printMonth = "JULY";
                       
                        break;
                    case "08":
                        printMonth = "AUGUST";
                       
                        break;
                    case "09":
                        printMonth = "SEPTEMBER";
                      
                        break;
                    case "10":
                        printMonth = "OCTOBER";
                       
                        break;
                    case "11":
                        printMonth = "NOVEMBER";
                       
                        break;
                    case "12":
                        printMonth = "DECEMBER";
                       
                        break;
                }
         

            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = "OUTWARD SUPPLY REGISTER OF CW ICD WALUJ";
                exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:M2", "TAX INVOICE / BILL OF SUPPLY FOR THE MONTH OF "+printMonth+" -2019", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A3:A4","SlNo", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B3:B4","Outward Type", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C3:C4","GSTIN of depositor/ customer" , DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D3:D4", "Name & Address of Customer to whom Service Rendered", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E3:E4", "Place of Supply", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F3:G3", "Invoice Details", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F4:F4", "Number", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G4:G4", "Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H3:H4", "HSN Code", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I3:I4", "Description of Supply", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J3:J4", "Rate of Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K3:k4", "Total Value of Supply Before Tax ", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L3:O3", "Amount of Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L4:L4", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M4:M4", "Central Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N4:N4", "State Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O4:O4", "Cess", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P3:P4", "RoundUp", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q3:Q4", "Total Amount" + Environment.NewLine + "(Taxable Value+ Taxes+ Cess+RoundUp)", DynamicExcel.CellAlignment.Middle);
                for (var i = 65; i < 80; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                }
                /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });*/
                exl.AddTable<Wlj_RegisterOfOutward>("A", 5, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });
                var servicevalue= model.Sum(o => o.ServiceValue);
                var igstamt = model.Sum(o => o.ITaxAmount);
                var sgstamt = model.Sum(o => o.STaxAmount);
                var cgstamt = model.Sum(o => o.CTaxAmount);
                var totalamt = model.Sum(o => o.Total);
                exl.AddCell("K" + (model.Count + 5).ToString(), servicevalue, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("L" + (model.Count + 5).ToString(), igstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("M" + (model.Count + 5).ToString(), sgstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("N" + (model.Count + 5).ToString(), cgstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (model.Count + 5).ToString(), totalamt, DynamicExcel.CellAlignment.CenterRight);
                /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/
                exl.Save();
            }
            return excelFile;
        }

        #endregion


        #region Details Of Import Export Container 
        public void GetImportExporContainer(int Month, int Year)
        {
            //DateTime dtfrom = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            //DateTime dtTo = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
           // String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //if (String.IsNullOrWhiteSpace(ObjBulkInvoiceReport.InvoiceModule))
            //{
            //    ObjBulkInvoiceReport.InvoiceModule = "";
            //}
            //if (String.IsNullOrWhiteSpace(ObjBulkInvoiceReport.InvoiceNumber))
            //{
            //    ObjBulkInvoiceReport.InvoiceNumber = "";
            //}
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.Int32,  Value =Month });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = Year });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceModule });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("ExpImPContainerDetails", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null)
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

        #region Register of E-Invoice 
        public void GetRegisterofEInvoice(DateTime date1, DateTime date2)
        {
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetRegisterofEInvoice", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];


            List<WLJRegisterOfEInvoiceModel> model = new List<WLJRegisterOfEInvoiceModel>();
            try
            {

                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = RegisterofEInvoiceExcel(model, dt);
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                ds.Dispose();
            }

        }


        private string RegisterofEInvoiceExcel(List<WLJRegisterOfEInvoiceModel> model, DataTable dt)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            System.Web.UI.WebControls.GridView Grid = new System.Web.UI.WebControls.GridView();


            System.Web.UI.WebControls.Table tb = new System.Web.UI.WebControls.Table();

            if (dt.Rows.Count > 0)
            {

                Grid.AllowPaging = false;
                Grid.DataSource = dt;
                Grid.DataBind();


                for (int i = 0; i < Grid.HeaderRow.Cells.Count; i++)
                {
                    Grid.HeaderRow.Cells[i].Style.Add("background-color", "#edebeb");
                    Grid.HeaderRow.Cells[i].Attributes.Add("class", "Headermode");


                }

                for (int i = 0; i < Grid.Rows.Count; i++)
                {
                    //Apply text style to each Row

                    Grid.Rows[i].BackColor = System.Drawing.Color.White;


                    //if (i % 2 != 0)
                    //{
                    //    Grid.Rows[i].Attributes.Add("class", "textmode");
                    //}
                    //else
                    //{
                    //    Grid.Rows[i].Attributes.Add("class", "textmode2");
                    //}

                }
                //var title = "CENTRAL WAREHOUSING CORPORATION </br>"
                //          + "Principal Place of Business</br>"
                //          + "CENTRAL WAREHOUSE</br>"
                //          + "REGISTER OF OUTWARD SUPPLY (TAX INVOICE/BILL OF SUPPLY)</br>";

                //System.Web.UI.WebControls.TableCell cell1 = new System.Web.UI.WebControls.TableCell();
                //cell1.Text = "<b> CENTRAL WAREHOUSING CORPORATION </b> ";
                //System.Web.UI.WebControls.TableRow tr1 = new System.Web.UI.WebControls.TableRow();
                //tr1.Cells.Add(cell1);
                //tr1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                //System.Web.UI.WebControls.TableCell cell2 = new System.Web.UI.WebControls.TableCell();
                //cell2.Text = "Principal Place of Business";
                //System.Web.UI.WebControls.TableRow tr2 = new System.Web.UI.WebControls.TableRow();
                //tr2.Cells.Add(cell2);
                //tr2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                //System.Web.UI.WebControls.TableCell cell3 = new System.Web.UI.WebControls.TableCell();
                //cell3.Text = "CENTRAL WAREHOUSE";
                //System.Web.UI.WebControls.TableRow tr3 = new System.Web.UI.WebControls.TableRow();
                //tr3.Cells.Add(cell3);
                //tr3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;


                //System.Web.UI.WebControls.TableCell cell4 = new System.Web.UI.WebControls.TableCell();
                //cell4.Text = "REGISTER OF OUTWARD SUPPLY (TAX INVOICE/BILL OF SUPPLY)";
                //System.Web.UI.WebControls.TableRow tr4 = new System.Web.UI.WebControls.TableRow();
                //tr4.Cells.Add(cell4);
                //tr4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;



                System.Web.UI.WebControls.TableCell cell5 = new System.Web.UI.WebControls.TableCell();
                cell5.Controls.Add(Grid);
                System.Web.UI.WebControls.TableRow tr5 = new System.Web.UI.WebControls.TableRow();
                tr5.Cells.Add(cell5);

                //tb.Rows.Add(tr1);
                //tb.Rows.Add(tr2);
                //tb.Rows.Add(tr3);
                //tb.Rows.Add(tr4);
                tb.Rows.Add(tr5);

            }
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                using (System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw))
                {
                    System.IO.StreamWriter writer = System.IO.File.AppendText(excelFile);
                    tb.RenderControl(hw);
                    writer.WriteLine(sw.ToString());
                    writer.Close();
                }
            }



            return excelFile;
        }


        #endregion

        #region Bulk EInvoice Generation

        public void GetBulkIrnDetails()
        {
            int Status = 0;

            IDataParameter[] DParam = { };

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            WLJ_BulkIRN objInvoice = new WLJ_BulkIRN();
            IDataReader Result = DataAccess.ExecuteDataReader("IRNNotgeneratedInvoiceList", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    objInvoice.lstPostPaymentChrg.Add(new WLJ_BulkIRNDetails
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        GstNo = Result["GstNo"].ToString(),
                        SupplyType = Result["SupplyType"].ToString(),
                        InvoiceType = Result["InvoiceType"].ToString(),
                        OperationType = Result["InvType"].ToString()
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

        public void AddEditIRNErrorResponse(string InvoiceNo, string ErrorMessage, int ErrorCode)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ErrorMessage", MySqlDbType = MySqlDbType.String, Value = ErrorMessage });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ErrorCode", MySqlDbType = MySqlDbType.Int32, Value = ErrorCode });


            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddeditirnErrorResponce", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "IRN Generate Successfully" : "IRN Generate Successfully";
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


        #region Register of Outward Supply NEW
        public void GetRegisterofOutwardSupplybyINVTYPE(DateTime date1, DateTime date2, string Type)
        {
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetRegisterofOutwardSupplyBYTYPE", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            if (Type == "Inv" || Type == "Unpaid" || Type == "CancelInv")
            {   
                List<PpgRegisterOfOutwardSupplyModel> model = new List<PpgRegisterOfOutwardSupplyModel>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        model = (from DataRow dr in dt.Rows
                                 select new PpgRegisterOfOutwardSupplyModel()
                                 {
                                     SlNo = Convert.ToInt32(dr["SlNo"]),
                                     GST = dr["GST"].ToString(),
                                     Place = dr["Place"].ToString(),
                                     Name = dr["Name"].ToString(),
                                     Period = dr["Period"].ToString(),

                                     Nature = dr["Nature"].ToString(),
                                     Rate = Convert.ToDecimal(dr["Rate"]),
                                     InvoiceNo = dr["InvoiceNo"].ToString(),
                                     InvoiceDate = dr["InvoiceDate"].ToString(),
                                     ServiceValue = Convert.ToDecimal(dr["ServiceValue"]),

                                     ITaxPercent = dr["ITaxPercent"].ToString(),
                                     ITaxAmount = Convert.ToDecimal(dr["ITaxAmount"]),
                                     CTaxPercent = dr["CTaxPercent"].ToString(),
                                     CTaxAmount = Convert.ToDecimal(dr["CTaxAmount"]),
                                     STaxPercent = dr["STaxPercent"].ToString(),
                                     STaxAmount = Convert.ToDecimal(dr["STaxAmount"]),

                                     Total = Convert.ToDecimal(dr["Total"]),
                                     //WH = dr["WH"].ToString(),
                                     //CRNoDate = dr["CRNoDate"].ToString(),
                                     //SD = Convert.ToDecimal(dr["SD"]),
                                     //Amount = Convert.ToDecimal(dr["Amount"]),
                                     //TDS = Convert.ToDecimal(dr["TDS"]),
                                     //ChequeNoDate = dr["CHDD"].ToString(),
                                     PaymentMode = dr["PaymentMode"].ToString(),
                                     Remarks = dr["Remarks"].ToString()
                                     ,
                                     HSNCode = dr["HSNCode"].ToString()
                                 }).ToList();
                    }
                    decimal InvoiceAmount = 0, CRAmount = 0;
                    //foreach (DataRow dr in ds.Tables[1].Rows)
                    //{
                    //    InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                    //    CRAmount = Convert.ToDecimal(dr["CRAmount"]);
                    //}
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = RegisterofOutwardSupplyExcelbYTYPE(model, InvoiceAmount, CRAmount);
                }
                catch (Exception ex)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
                finally
                {
                    ds.Dispose();
                }
            }
            else
            {
                List<PpgRegisterOfOutwardSupplyModelCreditDebit> modelCreditDebit = new List<PpgRegisterOfOutwardSupplyModelCreditDebit>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        modelCreditDebit = (from DataRow dr in dt.Rows
                                            select new PpgRegisterOfOutwardSupplyModelCreditDebit()
                                            {
                                                SlNo = Convert.ToInt32(dr["SlNo"]),
                                                GST = dr["GST"].ToString(),
                                                Place = dr["Place"].ToString(),
                                                Name = dr["Name"].ToString(),
                                                Period = dr["Period"].ToString(),

                                                Nature = dr["Nature"].ToString(),
                                                HSNCode = dr["HSNCode"].ToString(),
                                                Rate = Convert.ToDecimal(dr["Rate"]),
                                                CreditNoteNo = dr["CreditNoteNo"].ToString(),
                                                CRNoteDate = dr["CRNoteDate"].ToString(),
                                                InvoiceNo = dr["InvoiceNo"].ToString(),
                                                InvoiceDate = dr["InvoiceDate"].ToString(),
                                                ServiceValue = Convert.ToDecimal(dr["ServiceValue"]),

                                                ITaxPercent = dr["ITaxPercent"].ToString(),
                                                ITaxAmount = Convert.ToDecimal(dr["ITaxAmount"]),
                                                CTaxPercent = dr["CTaxPercent"].ToString(),
                                                CTaxAmount = Convert.ToDecimal(dr["CTaxAmount"]),
                                                STaxPercent = dr["STaxPercent"].ToString(),
                                                STaxAmount = Convert.ToDecimal(dr["STaxAmount"]),

                                                Total = Convert.ToDecimal(dr["Total"]),
                                                Remarks = dr["Remarks"].ToString()

                                            }).ToList();
                    }
                    decimal InvoiceAmount = 0, CRAmount = 0;
                    //foreach (DataRow dr in ds.Tables[1].Rows)
                    //{
                    //    InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                    //    CRAmount = Convert.ToDecimal(dr["CRAmount"]);
                    //}
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = RegisterofOutwardSupplyExcelCreditDebit(modelCreditDebit, InvoiceAmount, CRAmount);
                }
                catch (Exception ex)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
                finally
                {
                    ds.Dispose();
                }
            }


        }
        private string RegisterofOutwardSupplyExcelbYTYPE(List<PpgRegisterOfOutwardSupplyModel> model, decimal InvoiceAmount, decimal CRAmount)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "Principal Place of Business"
                        + Environment.NewLine + "CENTRAL WAREHOUSE"
                        + Environment.NewLine + Environment.NewLine
                        + "REGISTER OF OUTWARD SUPPLY (TAX INVOICE/BILL OF SUPPLY)";
                exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N1:O1", "BILL REGISTER", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:A4", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B2:B4", "GSTIN", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C2:C4", "Place" + Environment.NewLine + "(Name of State)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D2:D4", "Name of Customer to whom Service rendered", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E2:E4", "Period of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F2:F4", "Nature of Invoice" + Environment.NewLine + "(Resv./Initial Fumigatiom/General Basic/Over & Above)", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("G2:G4", "HSN Code", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H2:H4", "Rate per" + Environment.NewLine + "Bag/MT/Sqm", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I2:K2", "Invoice Details", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I3:I4", "Invoice No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J3:J4", "Date of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K3:K4", "Value of Service" + Environment.NewLine + "(Before Tax)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L2:Q2", "Rate of Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L3:M3", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N3:O3", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P3:Q3", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("L4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("M4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("N4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("O4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("P4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("Q4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R2:R4", "Total Invoice Value" + Environment.NewLine + "(18=(11+13 or 11+15+17))", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("R2:U2", "Perticulars of Payment Received", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("R3:R4", "Received" + Environment.NewLine + "At WH/RO", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("S3:S4", "C.R No. & Date", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("T3:T4", "Cheque/DD No. & Date", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("T3:T4", "SD", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("U3:U4", "Amount", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("V3:V4", "TDS", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("V2:V4", "Amount Received Against Bill (Rs.)", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("W2:W4", "Adjustment/Deduction", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("W2:W2", "Balance", DynamicExcel.CellAlignment.Middle);
                //exl.AddCell("W2", "Balance", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S2:S4", "PaymentMode", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T2:T4", "Remarks", DynamicExcel.CellAlignment.Middle);


                for (var i = 65; i < 85; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                }
                /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });/
                exl.AddTable<PpgRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });*/
                exl.AddTable<PpgRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 10, 30 });
                var igstamt = model.Sum(o => o.ITaxAmount);
                var sgstamt = model.Sum(o => o.STaxAmount);
                var cgstamt = model.Sum(o => o.CTaxAmount);
                var totalamt = model.Sum(o => o.Total);
                exl.AddCell("M" + (model.Count + 6).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 6).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (model.Count + 6).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("R" + (model.Count + 6).ToString(), totalamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/

                exl.Save();
            }
            return excelFile;
        }


        #endregion

        #region E04 Report

        public void ListofE04Report(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofE04Report", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Wlj_E04Report> LstE04 = new List<Wlj_E04Report>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstE04.Add(new Wlj_E04Report
                    {
                        ID = Convert.ToInt32(Result["ID"]),
                        CUSTOM_CD = Result["CUSTOM_CD"].ToString(),
                        SB_NO = Result["SB_NO"].ToString(),
                        SB_DATE = Result["SB_DATE"].ToString(),
                        IEC_CD = Result["IEC_CD"].ToString(),
                        BI_NO = Result["BI_NO"].ToString(),
                        EXP_NAME = Result["EXP_NAME"].ToString(),
                        Address = Result["Address"].ToString(),
                        CHA_CODE = Result["CHA_CODE"].ToString(),
                        FOB = Result["FOB"].ToString(),
                        POD = Result["POD"].ToString(),
                        LEO_NO = Result["LEO_NO"].ToString(),
                        LEO_DATE = Result["LEO_DATE"].ToString(),
                        ENTRY_NO = Result["ENTRY_NO"].ToString(),
                        G_DATE = Result["G_DATE"].ToString(),
                        TRANSHIPPER_CODE = Result["TRANSHIPPER_CODE"].ToString(),
                        GATEWAY_PORT = Result["GATEWAY_PORT"].ToString(),
                        PCIN = Result["PCIN"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstE04;
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

        public void GetE04DetailById(int ID)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ID", MySqlDbType = MySqlDbType.Int32, Value = ID });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("ViewE04Details", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Wlj_E04Report objE04Report = new Wlj_E04Report();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    objE04Report.ID = Convert.ToInt32(Result["ID"]);
                    objE04Report.CUSTOM_CD = Result["CUSTOM_CD"].ToString();
                    objE04Report.SB_NO = Result["SB_NO"].ToString();
                    objE04Report.SB_DATE = Result["SB_DATE"].ToString();
                    objE04Report.IEC_CD = Result["IEC_CD"].ToString();
                    objE04Report.BI_NO = Result["BI_NO"].ToString();
                    objE04Report.EXP_NAME = Result["EXP_NAME"].ToString();
                    objE04Report.EXP_ADD1 = Result["EXP_ADD1"].ToString();
                    objE04Report.EXP_ADD2 = Result["EXP_ADD2"].ToString();
                    objE04Report.PIN = Result["PIN"].ToString();
                    objE04Report.CITY = Result["CITY"].ToString();
                    objE04Report.CHA_CODE = Result["CHA_CODE"].ToString();
                    objE04Report.FOB = Result["FOB"].ToString();
                    objE04Report.POD = Result["POD"].ToString();
                    objE04Report.LEO_NO = Result["LEO_NO"].ToString();
                    objE04Report.LEO_DATE = Result["LEO_DATE"].ToString();
                    objE04Report.ENTRY_NO = Result["ENTRY_NO"].ToString();
                    objE04Report.G_DATE = Result["G_DATE"].ToString();
                    objE04Report.TRANSHIPPER_CODE = Result["TRANSHIPPER_CODE"].ToString();
                    objE04Report.GATEWAY_PORT = Result["GATEWAY_PORT"].ToString();
                    objE04Report.PCIN = Result["PCIN"].ToString();
                }

                if (Status == 1)
                {
                    _DBResponse.Data = objE04Report;
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

        public void GetE04DetailSearch(string SB_No, string SB_Date, string Exp_Name)
        {
            //string SBDate ="";
            //if (SB_Date != null && SB_Date != "" )
            //{
            //    DateTime SBDt = DateTime.ParseExact(SB_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //    SBDate = SBDt.ToString("yyyy-MM-dd");
            //}            

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.VarChar, Value = SB_No });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Exporter", MySqlDbType = MySqlDbType.VarChar, Value = Exp_Name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBDate", MySqlDbType = MySqlDbType.VarChar, Value = SB_Date });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofE04Search", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Wlj_E04Report> LstE04Report = new List<Wlj_E04Report>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstE04Report.Add(new Wlj_E04Report
                    {
                        ID = Convert.ToInt32(Result["ID"]),
                        CUSTOM_CD = Result["CUSTOM_CD"].ToString(),
                        SB_NO = Result["SB_NO"].ToString(),
                        SB_DATE = Result["SB_DATE"].ToString(),
                        IEC_CD = Result["IEC_CD"].ToString(),
                        BI_NO = Result["BI_NO"].ToString(),
                        EXP_NAME = Result["EXP_NAME"].ToString(),
                        Address = Result["Address"].ToString(),
                        CHA_CODE = Result["CHA_CODE"].ToString(),
                        FOB = Result["FOB"].ToString(),
                        POD = Result["POD"].ToString(),
                        LEO_NO = Result["LEO_NO"].ToString(),
                        LEO_DATE = Result["LEO_DATE"].ToString(),
                        ENTRY_NO = Result["ENTRY_NO"].ToString(),
                        G_DATE = Result["G_DATE"].ToString(),
                        TRANSHIPPER_CODE = Result["TRANSHIPPER_CODE"].ToString(),
                        GATEWAY_PORT = Result["GATEWAY_PORT"].ToString(),
                        PCIN = Result["PCIN"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstE04Report;
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
        #region Stuffing Acknowledgement Search       

        public void GetAllContainerNoForContstufserach(string cont, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_cont", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = cont });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = '0' });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNoForstufack", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            // List<StuffingRequestDtl> LstStuffing = new List<StuffingRequestDtl>();
            List<Wlj_ContStufAckSearch> LstStuffing = new List<Wlj_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Wlj_ContStufAckSearch
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
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
                    _DBResponse.Data = new { LstStuffing, State };
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
        public void GetAllShippingBillNoForContstufserach(string shipbill, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = shipbill });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = '0' });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetshippingbillNoForstufack", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            // List<StuffingRequestDtl> LstStuffing = new List<StuffingRequestDtl>();
            List<Wlj_ContStufAckSearch> LstStuff = new List<Wlj_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Wlj_ContStufAckSearch
                    {
                        shippingbillno = Result["ShippingBillNo"].ToString(),
                        shippingbilldate = Result["ShippingBillDate"].ToString(),
                        // ShippingLine = Result["ShippingLine"].ToString()
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
                    _DBResponse.Data = new { LstStuff, State };
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
        public void GetStufAckResult(string container, string shipbill)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_container", MySqlDbType = MySqlDbType.VarChar, Value = container });
            LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Value = shipbill });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStufAckResult", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Wlj_ContStufAckRes> Lststufack = new List<Wlj_ContStufAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Wlj_ContStufAckRes
                    {
                        shipbill = Result["shipbill"].ToString(),
                        reason = Result["reason"].ToString(),
                        status = Result["status"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lststufack;
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
