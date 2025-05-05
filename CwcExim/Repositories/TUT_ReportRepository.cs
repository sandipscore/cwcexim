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
using CwcExim.Areas.Auction.Models;
using CwcExim.Models;
using EinvoiceLibrary;

namespace CwcExim.Repositories
{ 
    public class TUT_ReportRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        #region Company details
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
                    objCompanyDetails.EmailAddress = Convert.ToString(Result["EmailAddress"]).Replace("<br/>", " ");
                    objCompanyDetails.EffectVersion = Convert.ToDecimal(Result["Version"]);
                    objCompanyDetails.VersionLogoFile = Convert.ToString(Result["Effectlogofile"]);
                    objCompanyDetails.BranchName = Convert.ToString(Result["BranchName"]);


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

        #region Monthly SD Book
        public void MonthSDBookReport(LNSM_DailyCashBook ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            
            int Status = 0;
            
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
           
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("MonthSDBookReport", CommandType.StoredProcedure, DParam);
            IList<LNSM_DailyCashBook> LstMonthlyCashBook = new List<LNSM_DailyCashBook>();           
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    
                    LstMonthlyCashBook.Add(new LNSM_DailyCashBook
                    {
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),

                        ReceiptDate = Result["ReceiptDate"].ToString(),
                        CRNo = Result["ReceiptNo"].ToString(),


                        InvoiceType = Result["InvoiceType"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        ModeOfPay = Result["ModeOfPay"].ToString(),

                        ChqNo = Result["ChequeNo"].ToString(),
                        MISC = Result["MISC"].ToString(),
                        MFCHRG = Result["MFCHRG"].ToString(),
                        INS = Result["INS"].ToString(),
                        GRL = Result["GRL"].ToString(),
                        TPT = Result["TPT"].ToString(),
                        EIC = Result["EIC"].ToString(),
                        THCCHRG = Result["THCCHRG"].ToString(),
                        GRE = Result["GRE"].ToString(),
                        RENT = Result["RENT"].ToString(),
                        RRCHRG = Result["RRCHRG"].ToString(),
                        STO = Result["STO"].ToString(),
                        TAC = Result["TAC"].ToString(),
                        TIS = Result["TIS"].ToString(),
                        GENSPACE = Result["GENSPACE"].ToString(),



                      //  Misc = Result["MISC"].ToString(),
                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),

                      //  MiscExcess = Result["MiscExcess"].ToString(),
                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        TotalOthers = Result["TotalOther"].ToString(),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString(),
                        TotalPDA = Result["TotalPDA"].ToString(),
                        Remarks = Result["Remarks"].ToString(),

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstMonthlyCashBook;
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

        #region DailyCashBook
        public void DailyCashBook(LNSM_DailyCashBook ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");            

            int Status = 0;
            
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });           

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyCashBookReport", CommandType.StoredProcedure, DParam);
            IList<LNSM_DailyCashBook> LstDailyCashBook = new List<LNSM_DailyCashBook>();           
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;                   

                    LstDailyCashBook.Add(new LNSM_DailyCashBook
                    {
                        
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),

                        ReceiptDate = Result["ReceiptDate"].ToString(),
                        CRNo = Result["ReceiptNo"].ToString(),


                        InvoiceType = Result["InvoiceType"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        ModeOfPay = Result["ModeOfPay"].ToString(),

                        ChqNo = Result["ChequeNo"].ToString(),



                        MISC = Result["MISC"].ToString(),
                        MFCHRG = Result["MFCHRG"].ToString(),
                        INS = Result["INS"].ToString(),
                        GRL = Result["GRL"].ToString(),
                        TPT = Result["TPT"].ToString(),
                        EIC = Result["EIC"].ToString(),
                        THCCHRG = Result["THCCHRG"].ToString(),
                        GRE = Result["GRE"].ToString(),
                        RENT = Result["RENT"].ToString(),
                        RRCHRG = Result["RRCHRG"].ToString(),
                        STO = Result["STO"].ToString(),
                        TAC = Result["TAC"].ToString(),
                        TIS = Result["TIS"].ToString(),
                        GENSPACE = Result["GENSPACE"].ToString(),
                       


                       
                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),

                      
                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        TotalOthers = Result["TotalOther"].ToString(),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString(),
                        TotalPDA = Result["TotalPDA"].ToString(),
                        Remarks = Result["Remarks"].ToString(),                        

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

   

        public void DailyCashBooKXls(LNSM_DailyCashBook ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyCashBookReportXLS", CommandType.StoredProcedure, DParam);
            IList<LNSM_DailyCashBookXLS> LstDailyCashBook = new List<LNSM_DailyCashBookXLS>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstDailyCashBook.Add(new LNSM_DailyCashBookXLS
                    {

                        //ReceiptNo, ReceiptDate, Party, ChequeNo, CashReceiptId, GenSpace, sto, Insurance, GroundRentEmpty, GroundRentLoaded, Mf, EntCharge, 
                        //Fum, OtCharge, CGSTAmt, SGSTAmt, IGSTAmt, MISC, MiscExcess, TotalCash, TotalCheque, tdsCol, crTDS
                        /*CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),
                        */
                        SLNO = Convert.ToInt32(Result["SLNO"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Result["ReceiptDate"].ToString(),



                     //   InvoiceType = Result["InvoiceType"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                      //  ModeOfPay = Result["ModeOfPay"].ToString(),

                        ChqNo = Result["ChequeNo"].ToString(),

                        MISC = Result["MISC"].ToString(),
                        MFCHRG = Result["MFCHRG"].ToString(),
                        INS = Result["INS"].ToString(),
                        GRL = Result["GRL"].ToString(),
                        TPT = Result["TPT"].ToString(),
                        EIC = Result["EIC"].ToString(),
                        THCCHRG = Result["THCCHRG"].ToString(),
                        GRE = Result["GRE"].ToString(),
                        RENT = Result["RENT"].ToString(),
                        RRCHRG = Result["RRCHRG"].ToString(),
                        STO = Result["STO"].ToString(),
                        TAC = Result["TAC"].ToString(),
                        TIS = Result["TIS"].ToString(),
                        GENSPACE = Result["GENSPACE"].ToString(),


                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),


                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        TotalOthers = Result["TotalOther"].ToString(),
                     //   TotalPDA = Result["TotalPDA"].ToString(),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString(),

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
                    DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    //  decimal InvoiceAmount = 0, CRAmount = 0;

                    _DBResponse.Data = DailyCashBookWithSdDetailXls(LstDailyCashBook, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"));

                }

                //if (Status == 1)
                //{
                //    _DBResponse.Status = 1;
                //    _DBResponse.Message = "Success";
                //    _DBResponse.Data = LstDailyCashBook;
                //}
                //else
                //{
                //    _DBResponse.Status = 0;
                //    _DBResponse.Message = "No Data";
                //    _DBResponse.Data = null;
                //}
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

        private string DailyCashBookWithSdDetailXls(IList<LNSM_DailyCashBookXLS> OBJDailyCashBookPpg, string date1, string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Govt. of India Undertaking)"
                        + Environment.NewLine + ""
                        + Environment.NewLine + Environment.NewLine
                        + "ExportContainer Income Detail";
                string typeOfValue = "";

                typeOfValue = "During Period Of " + date1 + " TO " + date2;



                exl.MargeCell("A1:H1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A2:H2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A3:H3", HttpContext.Current.Session["CompanayName"].ToString(), DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A4:H4", "Daily Cash Book With SD  " + typeOfValue, DynamicExcel.CellAlignment.Middle);
                /// exl.MargeCell("A5:H5", typeOfValue, DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("A4:F4", "DETAILS OF ORIGINAL INOICE", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("G4:O4", "DETAILS OF CREDIT NOTE ISSUED", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:A6", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B5:B6", "RECEIPT DATE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C6", "RECEIPT NO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D6", "INVOICENO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E6", "INVOICE DATE", DynamicExcel.CellAlignment.Middle);
             
                exl.MargeCell("F5:F6", "PARTY NAME.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G5:G6", "PAYEE NAME", DynamicExcel.CellAlignment.Middle);
               
                exl.MargeCell("H5:H6", "CHQ NO.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I5:I6", "MISC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J5:J6", "MF CHRG", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K5:K6", "INS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L5:L6", "GRL", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M5:M6", "TPT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N5:N6", "EIC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O5:O6", "THC CHRG", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P5:P6", "GRE ", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q5:Q6", "RENT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R5:R6", "RR CHRG", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S5:S6", "STO", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("T5:T6", "TAC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U5:U6", "TIS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("V5:V6", "GEN SPACE", DynamicExcel.CellAlignment.Middle);



                exl.MargeCell("W5:W6", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("X5:X6", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Y5:Y6", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Z5:Z6", "TOTAL CASH", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("AA5:AA6", "TOTAL CHQ", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AB5:AB6", "OTHERS", DynamicExcel.CellAlignment.Middle);
              
                exl.MargeCell("AC5:AC6", "TDS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AD5:AD6", "CR TDS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AE5:AE6", "REMARKS", DynamicExcel.CellAlignment.Middle);



                exl.AddTable("A", 7, OBJDailyCashBookPpg, new[] { 6, 20, 20, 30, 12, 20, 20, 15, 15, 30, 15, 20, 20, 20, 15, 15, 15, 10, 12, 18, 18, 16, 12, 12, 15, 15, 17, 18,15,15, 16 });

                var MISC = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.MISC)).ToString();
                var MFCHRG = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.MFCHRG)).ToString();
                var INS = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.INS)).ToString();
                var GRL = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.GRL)).ToString();
                var TPT = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.TPT)).ToString();
                var EIC = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.EIC)).ToString();
                var THCCHRG = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.THCCHRG)).ToString();
                var GRE = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.GRE)).ToString();
                var RENT = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.RENT)).ToString();

                var RRCHRG = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.RRCHRG)).ToString();
                var STO = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.STO)).ToString();
                var TAC = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.TAC)).ToString();
                var TIS = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.TIS)).ToString();
                var GENSPACE = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.GENSPACE)).ToString();
                var Cgst = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.Cgst)).ToString();
                var Sgst = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.Sgst)).ToString();
                var Igst = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.Igst)).ToString();

             






                var TotalCash = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.TotalCash)).ToString();
                var TotalCheque = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.TotalCheque)).ToString();
                var TotalOthers = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.TotalOthers)).ToString();
                var Tds = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.Tds)).ToString();
                var CrTds = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.CrTds)).ToString();
                var TotalPDA = 0;




                //var Total = PPGConIncomeDetail.Sum(o => o.Total);


                //var BOEValueDuty = PPGAssesmentSheetfcl.Sum(o => o.BOEValueDuty);
                exl.AddCell("H" + (OBJDailyCashBookPpg.Count + 7).ToString(), "Total", DynamicExcel.CellAlignment.TopLeft);

             
               
         

                exl.MargeCell("T5:T6", "TAC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U5:U6", "TIS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("V5:V6", "GEN SPACE", DynamicExcel.CellAlignment.Middle);

                exl.AddCell("I" + (OBJDailyCashBookPpg.Count + 7).ToString(), MISC.ToString(), DynamicExcel.CellAlignment.TopLeft);

                //exl.AddCell("W" + (PPGAssesmentSheetlcl.Count + 7).ToString(), Area.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("J" + (OBJDailyCashBookPpg.Count + 7).ToString(), MFCHRG.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("K" + (OBJDailyCashBookPpg.Count + 7).ToString(), INS.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("L" + (OBJDailyCashBookPpg.Count + 7).ToString(), GRL.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("M" + (OBJDailyCashBookPpg.Count + 7).ToString(), TPT.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("N" + (OBJDailyCashBookPpg.Count + 7).ToString(), EIC.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("O" + (OBJDailyCashBookPpg.Count + 7).ToString(), THCCHRG.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("AD" + (PPGAssesmentSheetfcl.Count + 7).ToString(), Haz.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.AddCell("P" + (OBJDailyCashBookPpg.Count + 7).ToString(), GRE.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("Q" + (OBJDailyCashBookPpg.Count + 7).ToString(), RENT.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("R" + (OBJDailyCashBookPpg.Count + 7).ToString(), RRCHRG.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("S" + (OBJDailyCashBookPpg.Count + 7).ToString(), STO.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.AddCell("T" + (OBJDailyCashBookPpg.Count + 7).ToString(), TAC.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("U" + (OBJDailyCashBookPpg.Count + 7).ToString(), TIS.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("V" + (OBJDailyCashBookPpg.Count + 7).ToString(), GENSPACE.ToString(), DynamicExcel.CellAlignment.TopLeft);


                exl.AddCell("W" + (OBJDailyCashBookPpg.Count + 7).ToString(), Cgst.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("X" + (OBJDailyCashBookPpg.Count + 7).ToString(), Sgst.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("Y" + (OBJDailyCashBookPpg.Count + 7).ToString(), Igst.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("Z" + (OBJDailyCashBookPpg.Count + 7).ToString(), TotalCash.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AA" + (OBJDailyCashBookPpg.Count + 7).ToString(), TotalCheque.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AB" + (OBJDailyCashBookPpg.Count + 7).ToString(), TotalOthers.ToString(), DynamicExcel.CellAlignment.TopLeft);

            //    exl.AddCell("Z" + (OBJDailyCashBookPpg.Count + 7).ToString(), TotalPDA.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.AddCell("AC" + (OBJDailyCashBookPpg.Count + 7).ToString(), Tds.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AD" + (OBJDailyCashBookPpg.Count + 7).ToString(), CrTds.ToString(), DynamicExcel.CellAlignment.TopLeft);


                ///exl.AddCell("AM" + (PPGConIncomeDetail.Count + 7).ToString(), TaxAmt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("AM" + (PPGConIncomeDetail.Count + 7).ToString(),Total.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.Save();
            }
            return excelFile;
        }


        public void MonthlySDBookXls(LNSM_DailyCashBook ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("MonthSDBookReportXls", CommandType.StoredProcedure, DParam);
            IList<LNSM_DailyCashBookXLS> LstDailyCashBook = new List<LNSM_DailyCashBookXLS>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstDailyCashBook.Add(new LNSM_DailyCashBookXLS
                    {

                        //ReceiptNo, ReceiptDate, Party, ChequeNo, CashReceiptId, GenSpace, sto, Insurance, GroundRentEmpty, GroundRentLoaded, Mf, EntCharge, 
                        //Fum, OtCharge, CGSTAmt, SGSTAmt, IGSTAmt, MISC, MiscExcess, TotalCash, TotalCheque, tdsCol, crTDS
                        /*CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),
                        */
                        SLNO = Convert.ToInt32(Result["SLNO"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Result["ReceiptDate"].ToString(),



                        //   InvoiceType = Result["InvoiceType"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        //  ModeOfPay = Result["ModeOfPay"].ToString(),

                        ChqNo = Result["ChequeNo"].ToString(),

                        MISC = Result["MISC"].ToString(),
                        MFCHRG = Result["MFCHRG"].ToString(),
                        INS = Result["INS"].ToString(),
                        GRL = Result["GRL"].ToString(),
                        TPT = Result["TPT"].ToString(),
                        EIC = Result["EIC"].ToString(),
                        THCCHRG = Result["THCCHRG"].ToString(),
                        GRE = Result["GRE"].ToString(),
                        RENT = Result["RENT"].ToString(),
                        RRCHRG = Result["RRCHRG"].ToString(),
                        STO = Result["STO"].ToString(),
                        TAC = Result["TAC"].ToString(),
                        TIS = Result["TIS"].ToString(),
                        GENSPACE = Result["GENSPACE"].ToString(),





                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),


                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        TotalOthers = Result["TotalOther"].ToString(),
                        //   TotalPDA = Result["TotalPDA"].ToString(),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString(),

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
                    DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    //  decimal InvoiceAmount = 0, CRAmount = 0;

                    _DBResponse.Data = MonthlySdBookDetailXls(LstDailyCashBook, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"));

                }

                //if (Status == 1)
                //{
                //    _DBResponse.Status = 1;
                //    _DBResponse.Message = "Success";
                //    _DBResponse.Data = LstDailyCashBook;
                //}
                //else
                //{
                //    _DBResponse.Status = 0;
                //    _DBResponse.Message = "No Data";
                //    _DBResponse.Data = null;
                //}
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

        private string MonthlySdBookDetailXls(IList<LNSM_DailyCashBookXLS> OBJDailyCashBookPpg, string date1, string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Govt. of India Undertaking)"
                        + Environment.NewLine + ""
                        + Environment.NewLine + Environment.NewLine
                        + "";
                string typeOfValue = "";

                typeOfValue = "During Period Of " + date1 + " TO " + date2;



                exl.MargeCell("A1:H1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A2:H2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A3:H3", HttpContext.Current.Session["CompanayName"].ToString(), DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A4:H4", "Monthly Cash Book With SD  " + typeOfValue, DynamicExcel.CellAlignment.Middle);
                /// exl.MargeCell("A5:H5", typeOfValue, DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("A4:F4", "DETAILS OF ORIGINAL INOICE", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("G4:O4", "DETAILS OF CREDIT NOTE ISSUED", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:A6", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B5:B6", "RECEIPT DATE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C6", "RECEIPT NO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D6", "INVOICENO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E6", "INVOICE DATE", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("F5:F6", "PARTY NAME.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G5:G6", "PAYEE NAME", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("H5:H6", "CHQ NO.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I5:I6", "MISC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J5:J6", "MF CHRG", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K5:K6", "INS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L5:L6", "GRL", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M5:M6", "TPT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N5:N6", "EIC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O5:O6", "THC CHRG", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P5:P6", "GRE ", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q5:Q6", "RENT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R5:R6", "RR CHRG", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S5:S6", "STO", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("T5:T6", "TAC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U5:U6", "TIS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("V5:V6", "GEN SPACE", DynamicExcel.CellAlignment.Middle);



                exl.MargeCell("W5:W6", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("X5:X6", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Y5:Y6", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Z5:Z6", "TOTAL CASH", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("AA5:AA6", "TOTAL CHQ", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AB5:AB6", "OTHERS", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("AC5:AC6", "TDS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AD5:AD6", "CR TDS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AE5:AE6", "REMARKS", DynamicExcel.CellAlignment.Middle);



                exl.AddTable("A", 7, OBJDailyCashBookPpg, new[] { 6, 20, 20, 30, 12, 20, 20, 15, 15, 30, 15, 20, 20, 20, 15, 15, 15, 10, 12, 18, 18, 16, 12, 12, 15, 15, 17, 18, 15, 15, 16 });

                var MISC = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.MISC)).ToString();
                var MFCHRG = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.MFCHRG)).ToString();
                var INS = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.INS)).ToString();
                var GRL = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.GRL)).ToString();
                var TPT = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.TPT)).ToString();
                var EIC = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.EIC)).ToString();
                var THCCHRG = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.THCCHRG)).ToString();
                var GRE = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.GRE)).ToString();
                var RENT = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.RENT)).ToString();

                var RRCHRG = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.RRCHRG)).ToString();
                var STO = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.STO)).ToString();
                var TAC = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.TAC)).ToString();
                var TIS = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.TIS)).ToString();
                var GENSPACE = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.GENSPACE)).ToString();
                var Cgst = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.Cgst)).ToString();
                var Sgst = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.Sgst)).ToString();
                var Igst = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.Igst)).ToString();








                var TotalCash = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.TotalCash)).ToString();
                var TotalCheque = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.TotalCheque)).ToString();
                var TotalOthers = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.TotalOthers)).ToString();
                var Tds = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.Tds)).ToString();
                var CrTds = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.CrTds)).ToString();
                var TotalPDA = 0;




                //var Total = PPGConIncomeDetail.Sum(o => o.Total);


                //var BOEValueDuty = PPGAssesmentSheetfcl.Sum(o => o.BOEValueDuty);
                exl.AddCell("H" + (OBJDailyCashBookPpg.Count + 7).ToString(), "Total", DynamicExcel.CellAlignment.TopLeft);





                exl.MargeCell("T5:T6", "TAC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U5:U6", "TIS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("V5:V6", "GEN SPACE", DynamicExcel.CellAlignment.Middle);

                exl.AddCell("I" + (OBJDailyCashBookPpg.Count + 7).ToString(), MISC.ToString(), DynamicExcel.CellAlignment.TopLeft);

                //exl.AddCell("W" + (PPGAssesmentSheetlcl.Count + 7).ToString(), Area.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("J" + (OBJDailyCashBookPpg.Count + 7).ToString(), MFCHRG.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("K" + (OBJDailyCashBookPpg.Count + 7).ToString(), INS.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("L" + (OBJDailyCashBookPpg.Count + 7).ToString(), GRL.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("M" + (OBJDailyCashBookPpg.Count + 7).ToString(), TPT.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("N" + (OBJDailyCashBookPpg.Count + 7).ToString(), EIC.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("O" + (OBJDailyCashBookPpg.Count + 7).ToString(), THCCHRG.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("AD" + (PPGAssesmentSheetfcl.Count + 7).ToString(), Haz.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.AddCell("P" + (OBJDailyCashBookPpg.Count + 7).ToString(), GRE.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("Q" + (OBJDailyCashBookPpg.Count + 7).ToString(), RENT.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("R" + (OBJDailyCashBookPpg.Count + 7).ToString(), RRCHRG.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("S" + (OBJDailyCashBookPpg.Count + 7).ToString(), STO.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.AddCell("T" + (OBJDailyCashBookPpg.Count + 7).ToString(), TAC.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("U" + (OBJDailyCashBookPpg.Count + 7).ToString(), TIS.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("V" + (OBJDailyCashBookPpg.Count + 7).ToString(), GENSPACE.ToString(), DynamicExcel.CellAlignment.TopLeft);


                exl.AddCell("W" + (OBJDailyCashBookPpg.Count + 7).ToString(), Cgst.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("X" + (OBJDailyCashBookPpg.Count + 7).ToString(), Sgst.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("Y" + (OBJDailyCashBookPpg.Count + 7).ToString(), Igst.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("Z" + (OBJDailyCashBookPpg.Count + 7).ToString(), TotalCash.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AA" + (OBJDailyCashBookPpg.Count + 7).ToString(), TotalCheque.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AB" + (OBJDailyCashBookPpg.Count + 7).ToString(), TotalOthers.ToString(), DynamicExcel.CellAlignment.TopLeft);

                //    exl.AddCell("Z" + (OBJDailyCashBookPpg.Count + 7).ToString(), TotalPDA.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.AddCell("AC" + (OBJDailyCashBookPpg.Count + 7).ToString(), Tds.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AD" + (OBJDailyCashBookPpg.Count + 7).ToString(), CrTds.ToString(), DynamicExcel.CellAlignment.TopLeft);


                ///exl.AddCell("AM" + (PPGConIncomeDetail.Count + 7).ToString(), TaxAmt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("AM" + (PPGConIncomeDetail.Count + 7).ToString(),Total.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.Save();
            }
            return excelFile;
        }



        #endregion

        #region DailyCashBookCash
        public void DailyCashBookCash(LNSM_DailyCashBook ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            
            int Status = 0;
           
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });           

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyCashBookReportCash", CommandType.StoredProcedure, DParam);
            IList<LNSM_DailyCashBook> LstDailyCashBook = new List<LNSM_DailyCashBook>();
           
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;                    

                    LstDailyCashBook.Add(new LNSM_DailyCashBook
                    {
                       
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        InvoiceType = Result["InvoiceType"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        ModeOfPay = Result["ModeOfPay"].ToString(),

                        ChqNo = Result["ChequeNo"].ToString(),
                        //GenSpace = Result["GenSpace"].ToString(),
                        //StorageCharge = Result["sto"].ToString(),
                        //Insurance = Result["Insurance"].ToString(),
                        //GroundRentEmpty = Result["GroundRentEmpty"].ToString(),
                        //GroundRentLoaded = Result["GroundRentLoaded"].ToString(),
                        //MfCharge = Result["Mf"].ToString(),
                        //EntryCharge = Result["EntCharge"].ToString(),
                        //Fumigation = Result["Fum"].ToString(),
                        //OtherCharge = Result["OtCharge"].ToString(),
                        //Misc = Result["MISC"].ToString(),
                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),

                      //  MiscExcess = Result["MiscExcess"].ToString(),
                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        TotalOthers = Result["TotalOther"].ToString(),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString(),
                        TotalPDA = Result["TotalPDA"].ToString(),
                        Remarks = Result["Remarks"].ToString(),                        

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

        #region MonthlyCashBook
        public void MonthlyCashBook(LNSM_DailyCashBook ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");           

            int Status = 0;
           
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });            

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("MonthlyCashBookReport", CommandType.StoredProcedure, DParam);
            IList<LNSM_DailyCashBook> LstMonthlyCashBook = new List<LNSM_DailyCashBook>();           
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                   

                    LstMonthlyCashBook.Add(new LNSM_DailyCashBook
                    {                        
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),
                       
                        //GenSpace = Result["GenSpace"].ToString(),
                        //StorageCharge = Result["sto"].ToString(),
                        //Insurance = Result["Insurance"].ToString(),
                        //GroundRentEmpty = Result["GroundRentEmpty"].ToString(),
                        //GroundRentLoaded = Result["GroundRentLoaded"].ToString(),
                        //MfCharge = Result["Mf"].ToString(),
                        //ThcCharge = Result["THC"].ToString(),
                        //RRCharge = Result["RR"].ToString(),
                        //FACCharge = Result["FAC"].ToString(),

                        //EntryCharge = Result["EntCharge"].ToString(),
                        //Fumigation = Result["Fum"].ToString(),
                        //OtherCharge = Result["OtCharge"].ToString(),
                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),
                        //Misc = Result["MISC"].ToString(),

                      //  MiscExcess = Result["MiscExcess"].ToString(),
                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        TotalOthers = Result["TotalOther"].ToString(),
                        TotalPDA = Result["TotalPDA"].ToString(),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString(),
                        TFUCharge = Result["TFU"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstMonthlyCashBook;
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

        #region ChequeCashDDPOSummary
        public void ChequeCashDDPOSummary(LNSM_CashChequeDDSummary ObjChequeSummary)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjChequeSummary.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjChequeSummary.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            

            int Status = 0;           
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });           

            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = ObjChequeSummary.Type });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CashCqeDdPoSdStmtRpt", CommandType.StoredProcedure, DParam);
            IList<LNSM_CashChequeDDSummary> LstChequeSummary = new List<LNSM_CashChequeDDSummary>();           
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstChequeSummary.Add(new LNSM_CashChequeDDSummary
                    {

                        Bank = Result["DraweeBank"].ToString(),
                        Type = Result["PayMode"].ToString(),
                        CashAmount = Convert.ToDecimal(Result["CASHAmount"]),
                        ChequeAmount = Convert.ToDecimal(Result["CHEQUEAmount"]),
                        POSAmount = Convert.ToDecimal(Result["POAmount"]),
                        Amount = Result["OtherAmount"].ToString(),
                        SDAmount = Convert.ToDecimal(Result["SDAmount"]),
                        OnlineAmount = Convert.ToDecimal(Result["OnlineAmount"]),

                        GCashAmount = Convert.ToDecimal(Result["CASHAmount"]),
                        GChequeAmount = Convert.ToDecimal(Result["CHEQUEAmount"]),
                        GPOSAmount = Convert.ToDecimal(Result["POAmount"]),
                        GOthersAmount = Convert.ToDecimal(Result["OtherAmount"]),
                        GOnlineAmount = Convert.ToDecimal(Result["OnlineAmount"]),
                        ChequeNumber = Result["chequeNumber"].ToString(),
                        Date = Result["Date"].ToString(),
                        Party = Result["Party"].ToString(),
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString()                       

                    });
                }
                if (Status == 1)
                {
                    LstChequeSummary.Add(new LNSM_CashChequeDDSummary
                    {



                        Bank = string.Empty,

                        GOthersAmount = LstChequeSummary.ToList().Sum(m => Convert.ToDecimal(m.GOthersAmount)),
                        GCashAmount = LstChequeSummary.ToList().Sum(m => m.GCashAmount),
                        GChequeAmount = LstChequeSummary.ToList().Sum(m => m.ChequeAmount),
                        GPOSAmount = LstChequeSummary.ToList().Sum(m => m.GPOSAmount),
                        GOnlineAmount = LstChequeSummary.ToList().Sum(m => m.GOnlineAmount),
                        SDAmount = LstChequeSummary.ToList().Sum(m => m.SDAmount),

                        Amount = string.Empty,


                        ChequeNumber = "<strong>Total</strong>",
                        Date = string.Empty,
                        Party = string.Empty,
                        InvoiceNumber = string.Empty,
                        ReceiptNo = string.Empty
                       
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

        public void ChequeCashDDPOSummaryExcel(LNSM_CashChequeDDSummary ObjChequeSummary)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjChequeSummary.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjChequeSummary.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");


            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = ObjChequeSummary.Type });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CashCqeDdPoSdStmtRptExcel", CommandType.StoredProcedure, DParam);
            IList<LNSM_CashChequeDDSummaryExcel> LstChequeSummary = new List<LNSM_CashChequeDDSummaryExcel>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstChequeSummary.Add(new LNSM_CashChequeDDSummaryExcel
                    {
                        SLNo = Result["RowNumber"].ToString(),
                        Bank = Result["DraweeBank"].ToString(),
                       // Type = Result["PayMode"].ToString(),
                        CashAmount = Convert.ToDecimal(Result["CASHAmount"]),
                        ChequeAmount = Convert.ToDecimal(Result["CHEQUEAmount"]),
                        POSAmount = Convert.ToDecimal(Result["POAmount"]),
                        OtherAmount = Convert.ToDecimal(Result["OtherAmount"].ToString()),
                      //  SDAmount = Convert.ToDecimal(Result["SDAmount"]),
                        OnlineAmount = Convert.ToDecimal(Result["OnlineAmount"]),

                        GCashAmount = Convert.ToDecimal(Result["CASHAmount"]),
                        GChequeAmount = Convert.ToDecimal(Result["CHEQUEAmount"]),
                        GPOSAmount = Convert.ToDecimal(Result["POAmount"]),
                        GOthersAmount = Convert.ToDecimal(Result["OtherAmount"]),
                        GOnlineAmount = Convert.ToDecimal(Result["OnlineAmount"]),
                        ChequeNumber = Result["chequeNumber"].ToString(),
                        Date = Result["Date"].ToString(),
                        Party = Result["Party"].ToString(),
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString()

                    });
                }
               

                if (Status == 1)
                {

                    foreach(var data in LstChequeSummary)
                    {
                        if(data.InvoiceNumber=="")
                        {
                            data.SDCashAmount = data.CashAmount;
                            data.SDChequeAmount = data.ChequeAmount;
                            data.SDPOSAmount = data.POSAmount;
                            data.SDOtherAmount = data.OtherAmount;
                            data.SDOnlineAmount = data.OnlineAmount;
                            data.CashAmount = 0;
                            data.ChequeAmount=0;
                            data.POSAmount = 0;
                            data.OtherAmount = 0;
                            data.OnlineAmount = 0;
                        }
                        else
                        {
                            data.SDCashAmount = 0;
                            data.SDChequeAmount = 0;
                            data.SDPOSAmount = 0;
                            data.SDOtherAmount = 0;
                            data.SDOnlineAmount =0 ;
                        }


                    }



                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CashChequeDDSummeryXls(LstChequeSummary, ObjChequeSummary.PeriodFrom, ObjChequeSummary.PeriodTo);
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

        private string CashChequeDDSummeryXls(IList<LNSM_CashChequeDDSummaryExcel> OBJDailyCashBookPpg, string date1, string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Govt. of India Undertaking)"
                        + Environment.NewLine + ""
                        + Environment.NewLine + Environment.NewLine
                        + "";
                string typeOfValue = "";

                typeOfValue = "STATEMENT OF CASH/CHEQUE/POS ETC SUMMARY REPORT FROM DATE " + date1 + " TO DATE" + date2;
                
                exl.MargeCell("A1:V1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A2:V2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A3:V3", HttpContext.Current.Session["CompanayName"].ToString(), DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A4:V4", typeOfValue, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A5:A6", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B5:B6", "Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C6", "Party", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D6", "Invoice No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E6", "Receipt No", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("F5:F6", "Bank Name", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G5:G6", "Cheque Number", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H5:L5", "Invoice Amount", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("H6:H6", "Cash", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I6:I6", "Cheque", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J6:J6", "Pos", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K6:K6", "Online", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L6:L6", "OTHERS(Draft /Challan etc)", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("M5:Q5", "SD Receipt Amount", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("M6:M6", "Cash", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N6:N6", "Cheque", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O6:O6", "Pos", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P6:P6", "Online", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q6:Q6", "OTHERS(Draft /Challan etc)", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("R5:V5", "Grand Total", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("R6:R6", "Cash", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S6:S6", "Cheque", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T6:T6", "Pos", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U6:U6", "Online", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("V6:V6", "OTHERS(Draft /Challan etc)", DynamicExcel.CellAlignment.Middle);


             


                exl.AddTable("A", 7, OBJDailyCashBookPpg, new[] { 6, 10, 30, 30, 20, 20, 20, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 });

                var InCash = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.CashAmount)).ToString();
                var InCheq = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.ChequeAmount)).ToString();
                var InPo = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.POSAmount)).ToString();
                var InOnline = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.OnlineAmount)).ToString();
                var InOther = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.OtherAmount)).ToString();

                var SDCash = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.SDCashAmount)).ToString();
                var SDCheq = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.SDChequeAmount)).ToString();
                var SDPo = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.SDPOSAmount)).ToString();
                var SDOnline = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.SDOnlineAmount)).ToString();
                var SDOther = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.SDOtherAmount)).ToString();



                var GNCash = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.GCashAmount)).ToString();
                var GNCheq = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.GChequeAmount)).ToString();
                var GNPo = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.GPOSAmount)).ToString();
                var GNOnline = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.GOnlineAmount)).ToString();
                var GNOther = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.GOthersAmount)).ToString();





                exl.AddCell("H" + (OBJDailyCashBookPpg.Count + 7).ToString(), InCash.ToString(), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("I" + (OBJDailyCashBookPpg.Count + 7).ToString(), InCheq.ToString(), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("J" + (OBJDailyCashBookPpg.Count + 7).ToString(), InPo.ToString(), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("K" + (OBJDailyCashBookPpg.Count + 7).ToString(), InOnline.ToString(), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("L" + (OBJDailyCashBookPpg.Count + 7).ToString(), InOther.ToString(), DynamicExcel.CellAlignment.TopRight);

                exl.AddCell("M" + (OBJDailyCashBookPpg.Count + 7).ToString(), SDCash.ToString(), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("N" + (OBJDailyCashBookPpg.Count + 7).ToString(), SDCheq.ToString(), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("O" + (OBJDailyCashBookPpg.Count + 7).ToString(), SDPo.ToString(), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("P" + (OBJDailyCashBookPpg.Count + 7).ToString(), SDOnline.ToString(), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("Q" + (OBJDailyCashBookPpg.Count + 7).ToString(), SDOther.ToString(), DynamicExcel.CellAlignment.TopRight);

                exl.AddCell("R" + (OBJDailyCashBookPpg.Count + 7).ToString(), GNCash.ToString(), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("S" + (OBJDailyCashBookPpg.Count + 7).ToString(), GNCheq.ToString(), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("T" + (OBJDailyCashBookPpg.Count + 7).ToString(), GNPo.ToString(), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("U" + (OBJDailyCashBookPpg.Count + 7).ToString(), GNOnline.ToString(), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("V" + (OBJDailyCashBookPpg.Count + 7).ToString(), GNOther.ToString(), DynamicExcel.CellAlignment.TopRight);



                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region SD Details Statement
        public void GetAllPartyForSDDet(string PartyCode, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();           
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
            LNSM_SDDetailsStatement SDResult = new LNSM_SDDetailsStatement();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    SDResult.PartyName = Result["PartyName"].ToString();
                    SDResult.PartyCode = Result["PartyCode"].ToString();
                    SDResult.PartyGst = Result["PartyGst"].ToString();
                    SDResult.CompanyGst = Result["CompanyGst"].ToString();                   

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        SDResult.lstInvc.Add(
                            new LNSM_SDInvoiceDet
                            {
                               
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

        public void GetSDDetStatementExcel(LNSM_SDDetReport Obj)
        {
            int Status = 0;

            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SDPartyId", MySqlDbType = MySqlDbType.Int32, Value = Obj.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fdt", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Tdt", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSDDetailsStatement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            LNSM_SDDetailsStatement SDResult = new LNSM_SDDetailsStatement();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    SDResult.PartyName = Result["PartyName"].ToString();
                    SDResult.PartyCode = Result["PartyCode"].ToString();
                    SDResult.PartyGst = Result["PartyGst"].ToString();
                    SDResult.CompanyGst = Result["CompanyGst"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        SDResult.lstInvc.Add(
                            new LNSM_SDInvoiceDet
                            {

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

                DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                
                _DBResponse.Data = SDDetStatementExcel(SDResult, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"));

                //if (Status == 1)
                //{
                //    _DBResponse.Status = 1;
                //    _DBResponse.Message = "Success";
                //    _DBResponse.Data = SDResult;
                //}
                //else
                //{
                //    _DBResponse.Status = 0;
                //    _DBResponse.Message = "No Data";
                //    _DBResponse.Data = null;
                //}

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

        private string SDDetStatementExcel(LNSM_SDDetailsStatement LSDetail, string date1, string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Govt. of India Undertaking)"
                        + Environment.NewLine + "ICD Loni"
                        + Environment.NewLine + Environment.NewLine
                        + "SD Report";
                string typeOfValue = "";

                typeOfValue = "During Period Of " + date1 + " TO " + date2;



                exl.MargeCell("A1:H1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A2:H2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A3:H3", HttpContext.Current.Session["CompanayName"].ToString(), DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A4:H4", "SD Report Detail", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:H5", typeOfValue, DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("A4:F4", "DETAILS OF ORIGINAL INOICE", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("G4:O4", "DETAILS OF CREDIT NOTE ISSUED", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A6:H6", "Party Name : " + LSDetail.PartyName, DynamicExcel.CellAlignment.CenterLeft);
                exl.MargeCell("A7:H7", "Folio No. : " + LSDetail.PartyCode, DynamicExcel.CellAlignment.CenterLeft);
                exl.MargeCell("A8:H8", "CWC GST No. : " + LSDetail.CompanyGst, DynamicExcel.CellAlignment.CenterLeft);
                exl.MargeCell("A9:H9", "Party GST No. : " + LSDetail.PartyGst, DynamicExcel.CellAlignment.CenterLeft);
                exl.MargeCell("A10:H10", "Period : " + date1 + " TO " + date2, DynamicExcel.CellAlignment.CenterLeft);

                exl.MargeCell("A11:A11", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B11:B11", "InvoiceNo", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C11:C11", "InvoiceDate", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D11:D11", "Receipt No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E11:E11", "Receipt Date.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F11:F11", "Pay Receipt", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G11:G11", "Transaction Type", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H11:H11", "Transaction Amount", DynamicExcel.CellAlignment.Middle);

        
                exl.AddTable("A", 12, LSDetail.lstInvc, new[] { 6, 30, 30, 30, 30, 30, 20, 30 });

                int RowCount = LSDetail.lstInvc.Count();


                exl.MargeCell("A" + (RowCount + 12).ToString() + ":" + "H" + (RowCount + 12).ToString(), "Invoice Utilization Balance : " + LSDetail.UtilizationAmount, DynamicExcel.CellAlignment.TopRight);
                
                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region DailyPdaActivityReport
        public void DailyPdaActivity(LNSM_DailyPdaActivityReport ObjDailyPdaActivityReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            
            int Status = 0;
           
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });           

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailySdActivityReport", CommandType.StoredProcedure, DParam);
            IList<LNSM_DailyPdaActivityReport> LstDailyPdaActivityReport = new List<LNSM_DailyPdaActivityReport>();           
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;                    

                    LstDailyPdaActivityReport.Add(new LNSM_DailyPdaActivityReport
                    {                       
                        partycode = Result["PartyCode"].ToString(),
                        Party = Result["PartyName"].ToString(),
                        Opening = Convert.ToDecimal(Result["OpeningAmount"]),
                        Deposit = Result["DebitAmount"].ToString(),
                        Withdraw = Result["AdjustAmount"].ToString(),
                        Closing = Convert.ToDecimal(Result["UtilizationAmount"]),


                    });
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
        public void DailyPdaActivityExcel(LNSM_DailyPdaActivityReport ObjDailyPdaActivityReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailySdActivityReportExcel", CommandType.StoredProcedure, DParam);
            List<LNSM_DailyPdaActivityReportExcel> LstDailyPdaActivityReport = new List<LNSM_DailyPdaActivityReportExcel>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstDailyPdaActivityReport.Add(new LNSM_DailyPdaActivityReportExcel
                    {
                         RowNumber= Convert.ToInt32(Result["OpeningAmount"]),
                        partycode = Result["PartyCode"].ToString(),
                        Party = Result["PartyName"].ToString(),
                        Opening = Convert.ToDecimal(Result["OpeningAmount"]),
                        Deposit = Result["DebitAmount"].ToString(),
                        Withdraw = Result["AdjustAmount"].ToString(),
                        Closing = Convert.ToDecimal(Result["UtilizationAmount"]),


                    });
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = GenerateDailyPdaActivityExcel(LstDailyPdaActivityReport, ObjDailyPdaActivityReport.PeriodFrom, ObjDailyPdaActivityReport.PeriodTo);
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

        private string GenerateDailyPdaActivityExcel(List<LNSM_DailyPdaActivityReportExcel> LSDetail, string date1, string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Govt. of India Undertaking)"
                        + Environment.NewLine + "ICD Loni"
                        + Environment.NewLine + Environment.NewLine
                        + "SD Report";
                string typeOfValue = "";

                typeOfValue = "Daily SD Activity Report From " + date1 + " TO " + date2;



                exl.MargeCell("A1:G1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A2:G2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A3:G3", HttpContext.Current.Session["CompanayName"].ToString(), DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A4:G4", typeOfValue, DynamicExcel.CellAlignment.Middle);
           
                
                exl.MargeCell("A5:A5", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B5:B5", "Party Code", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C5", "Party Name", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D5", "Opening Balance", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E5", "Dr. Amount", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F5:F5", "Cr. Amount", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G5:G5", "Closing Balance", DynamicExcel.CellAlignment.Middle);
             


                exl.AddTable("A", 6, LSDetail, new[] { 6, 30, 30, 10, 10, 10,10 });

                int RowCount = LSDetail.Count();

                decimal OpeningAmount = LSDetail.Sum(x => Convert.ToDecimal(x.Opening));
                decimal ClosingAmount = LSDetail.Sum(x => Convert.ToDecimal(x.Closing));
                decimal DrAmount = LSDetail.Sum(x => Convert.ToDecimal(x.Withdraw));
                decimal CrAmount = LSDetail.Sum(x => Convert.ToDecimal(x.Deposit));

                exl.MargeCell("A" + (RowCount + 6).ToString()+ ":C" + (RowCount + 6).ToString(), "Total", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D" + (RowCount + 6).ToString() + ":D" + (RowCount + 6).ToString(), OpeningAmount, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E" + (RowCount + 6).ToString() + ":E" + (RowCount + 6).ToString(), CrAmount, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F" + (RowCount + 6).ToString() + ":F" + (RowCount + 6).ToString(), DrAmount, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G" + (RowCount + 6).ToString() + ":G" + (RowCount + 6).ToString(), ClosingAmount, DynamicExcel.CellAlignment.Middle);
                exl.Save();
            }
            return excelFile;
        }
        #endregion

        #region PDUtilizationSummary
        public void PdSummaryUtilizationReport(LNSM_PdSummary ObjPdSummaryReport, int type = 1)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjPdSummaryReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
           
            int Status = 0;
            
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_asOndate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.Int32, Value = type });            

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PdSummaryUtilizationReport", CommandType.StoredProcedure, DParam);
            IList<LNSM_PdSummary> LstPdSummaryReport = new List<LNSM_PdSummary>();            
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    
                    LstPdSummaryReport.Add(new LNSM_PdSummary
                    {
                        PartyName = Result["PartyName"].ToString(),
                        Amount = Result["Amount"].ToString()
                    });
                }

                LstPdSummaryReport.Add(new LNSM_PdSummary
                {
                    PartyName = "Total",
                    Amount = LstPdSummaryReport.Sum(x => Convert.ToDecimal(x.Amount)).ToString()
                });

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

        #region CashReceiptInvoiceLedgerReport Partywise
        public void GetPaymentParty()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentParty", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<LNSM_PaymentPartyName> objPaymentPartyName = new List<LNSM_PaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new LNSM_PaymentPartyName()
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
            LNSM_CashReceiptInvoiceLedger CrInvLedgerObj = new LNSM_CashReceiptInvoiceLedger();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {

                    CrInvLedgerObj.OpenningBalance = Convert.ToDecimal(Result["OpenningBalance"]);
                    CrInvLedgerObj.EximTraderName = Convert.ToString(Result["EximTraderName"]);
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
                        CrInvLedgerObj.lstLedgerSummary.Add(new LNSM_CrInvLedgerSummary
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
                        CrInvLedgerObj.lstLedgerDetails.Add(new LNSM_CrInvLedgerDetails
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
                        CrInvLedgerObj.lstLedgerDetailsFull.Add(new LNSM_CrInvLedgerFullDetails
                        {
                            Sr = Convert.ToInt32(Result["Sr"]),                            
                            ReceiptDt = Convert.ToString(Result["ReceiptDt"]),
                            ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                            ChargeCode = Convert.ToString(Result["ChargeCode"]),
                            ContNo = Convert.ToString(Result["ContNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            Debit = Convert.ToDecimal(Result["Debit"]),
                            Credit = Convert.ToDecimal(Result["Credit"]),
                            Balance = Convert.ToDecimal(Result["Balance"]),
                            GroupSr = Convert.ToString(Result["GroupSr"]),


                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        CrInvLedgerObj.RoundUp= Convert.ToDecimal(Result["RoundUp"]);
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

                    CrInvLedgerObj.TotalDebit = CrInvLedgerObj.lstLedgerDetailsFull.Sum(x => x.Debit)+ CrInvLedgerObj.RoundUp;
                    CrInvLedgerObj.TotalCredit = CrInvLedgerObj.lstLedgerDetailsFull.Sum(x => x.Credit);
                    CrInvLedgerObj.ClosingBalance = (CrInvLedgerObj.OpenningBalance + CrInvLedgerObj.TotalCredit) - (CrInvLedgerObj.TotalDebit);                    
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

        #region Register of Outward Supply
        public void GetRegisterofOutwardSupply(DateTime date1, DateTime date2, string Type)
        {
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
           
            DataSet ds = DataAccess.ExecuteDataSet("GetRegisterofOutwardSupply", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            if (Type == "Inv"|| Type == "CancelInv")
            {
                List<LNSM_RegisterOfOutwardSupplyModel> model = new List<LNSM_RegisterOfOutwardSupplyModel>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        model = (from DataRow dr in dt.Rows
                                 select new LNSM_RegisterOfOutwardSupplyModel()
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
                                   
                                     PaymentMode = dr["PaymentMode"].ToString(),
                                     Remarks = dr["Remarks"].ToString()
                                     ,
                                     HSNCode = dr["HSNCode"].ToString()
                                 }).ToList();
                    }
                    decimal InvoiceAmount = 0, CRAmount = 0;
                   
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
            else if(Type == "Unpaid")
            {
                List<LNSM_RegisterOfOutwardUpaidModel> model = new List<LNSM_RegisterOfOutwardUpaidModel>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        model = (from DataRow dr in dt.Rows
                                 select new LNSM_RegisterOfOutwardUpaidModel()
                                 {
                                     SlNo = Convert.ToInt32(dr["SlNo"]),
                                     GST = dr["GST"].ToString(),
                                     Place = dr["Place"].ToString(),
                                     Name = dr["Name"].ToString(),  
                                     InvoiceNo = dr["InvoiceNo"].ToString(),
                                     InvoiceDate = dr["InvoiceDate"].ToString(), 
                                     Total = Convert.ToDecimal(dr["Total"]),                                  
                                     Remarks = dr["Remarks"].ToString()                                     ,
                                    
                                 }).ToList();
                    }
                    decimal InvoiceAmount = 0, CRAmount = 0;

                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = RegisterofOutwardSupplyUnpaidExcel(model, InvoiceAmount, CRAmount);
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
                List<LNSM_RegisterOfOutwardSupplyModelCreditDebit> modelCreditDebit = new List<LNSM_RegisterOfOutwardSupplyModelCreditDebit>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        modelCreditDebit = (from DataRow dr in dt.Rows
                                            select new LNSM_RegisterOfOutwardSupplyModelCreditDebit()
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
        private string RegisterofOutwardSupplyExcel(List<LNSM_RegisterOfOutwardSupplyModel> model, decimal InvoiceAmount, decimal CRAmount)
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
                exl.MargeCell("R2:R4", "Total Invoice Value" + Environment.NewLine + "(18=(11+13 or 11+15+17))", DynamicExcel.CellAlignment.Middle);
               
                exl.MargeCell("S2:S4", "PaymentMode", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T2:T4", "Remarks", DynamicExcel.CellAlignment.Middle);


                for (var i = 65; i < 85; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                }
               
                exl.AddTable<LNSM_RegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 10, 30 });
                var serviceValue = model.Sum(o => o.ServiceValue);
                var igstamt = model.Sum(o => o.ITaxAmount);
                var sgstamt = model.Sum(o => o.STaxAmount);
                var cgstamt = model.Sum(o => o.CTaxAmount);
                var totalamt = model.Sum(o => o.Total);
                exl.AddCell("K" + (model.Count + 6).ToString(), serviceValue.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("M" + (model.Count + 6).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 6).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (model.Count + 6).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("R" + (model.Count + 6).ToString(), totalamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
              

                exl.Save();
            }
            return excelFile;
        }
        private string RegisterofOutwardSupplyUnpaidExcel(List<LNSM_RegisterOfOutwardUpaidModel> model, decimal InvoiceAmount, decimal CRAmount)
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
                exl.MargeCell("A1:G1", title, DynamicExcel.CellAlignment.Middle);
               // exl.MargeCell("N1:O1", "BILL REGISTER", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:A4", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B2:B4", "GSTIN", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C2:C4", "Place" + Environment.NewLine + "(Name of State)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D2:D4", "Name of Customer to whom Service rendered", DynamicExcel.CellAlignment.Middle);              
                exl.MargeCell("E3:E4", "Invoice No.", DynamicExcel.CellAlignment.Middle);              
                exl.MargeCell("F2:F4", "Total Invoice Value", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G2:G4", "Remarks", DynamicExcel.CellAlignment.Middle);


                for (var i = 65; i < 72; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                }

                exl.AddTable<LNSM_RegisterOfOutwardUpaidModel>("A", 6, model, new[] { 6, 20, 20, 20, 12, 20, 10, 30 });
              
                var totalamt = model.Sum(o => o.Total);
              
                exl.AddCell("F" + (model.Count + 6).ToString(), totalamt.ToString(), DynamicExcel.CellAlignment.CenterRight);


                exl.Save();
            }
            return excelFile;
        }
        private string RegisterofOutwardSupplyExcelCreditDebit(List<LNSM_RegisterOfOutwardSupplyModelCreditDebit> modelCreditDebit, decimal InvoiceAmount, decimal CRAmount)
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
                 

                for (var i = 65; i < 86; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                }
                
                exl.AddTable("A", 6, modelCreditDebit, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 15, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 30 });

                var service= modelCreditDebit.Sum(o => o.ServiceValue);
                var igstamt = modelCreditDebit.Sum(o => o.ITaxAmount);
                var sgstamt = modelCreditDebit.Sum(o => o.STaxAmount);
                var cgstamt = modelCreditDebit.Sum(o => o.CTaxAmount);
                var totalamt = modelCreditDebit.Sum(o => o.Total);
                exl.AddCell("M" + (modelCreditDebit.Count + 6).ToString(), service.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (modelCreditDebit.Count + 6).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (modelCreditDebit.Count + 6).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("S" + (modelCreditDebit.Count + 6).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("T" + (modelCreditDebit.Count + 6).ToString(), totalamt.ToString(), DynamicExcel.CellAlignment.CenterRight);                

                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region Invoice Information

        public void GetInvPaymentParty( int Page=0, string SearchValue="")
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvPaymentParty", CommandType.StoredProcedure, DParam);
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
        public void GetAllInvoiceInformation(int Page, string PeriodFrom, string PeriodTo, string invNo, string PartyGSTNo, string PartyName, string RefInvoiceNo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                var LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.VarChar, Value = Page });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom==""? null : Convert.ToDateTime(PeriodFrom).ToString("yyyy-MM-dd") });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo == "" ? null : Convert.ToDateTime(PeriodTo).ToString("yyyy-MM-dd") });
                LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = invNo });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PartyGSTNo", MySqlDbType = MySqlDbType.VarChar, Value = PartyGSTNo });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = PartyName });
                LstParam.Add(new MySqlParameter { ParameterName = "in_RefInvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = RefInvoiceNo });

                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                _DBResponse = new DatabaseResponse();               
                Result = DataAccess.ExecuteDataSet("GetAllInvoiceInformation", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<LNSM_InvoiceInformation> InvcList = new List<LNSM_InvoiceInformation>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        LNSM_InvoiceInformation objInv = new LNSM_InvoiceInformation();
                        objInv.InvoiceId = Convert.ToInt32(dr["InvoiceId"]);
                        objInv.InvoiceNumber = Convert.ToString(dr["InvoiceNo"]);
                        objInv.RefInvoiceNo = Convert.ToString(dr["RefInvoiceNo"]);
                        objInv.InvoiceDate = Convert.ToString(dr["InvoiceDate"]);
                        objInv.PartyGSTNo = Convert.ToString(dr["PartyGSTNo"]);
                        objInv.PartyName = Convert.ToString(dr["PartyName"]);
                        objInv.TotalAmt = Convert.ToDecimal(dr["TotalAmt"]);
                        InvcList.Add(objInv);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = InvcList;
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

        public void GenericInvoiceDetailsForPrint(LNSM_InvoiceInformation ObjInvoice)
        {            
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjInvoice.InvoiceId });
           
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GenericInvoiceDetailsForPrint", CommandType.StoredProcedure, DParam);
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

        #region Add edit QR Code
        public void AddEditBQRCode(int InvoiceId, string FileName, int CreatedBy=0)
        {
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FileName", MySqlDbType = MySqlDbType.VarChar, Value = FileName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.VarChar, Value = CreatedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("AddEdithdfcbqrcode", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Saved Successfully.";
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

        #region Daily Transaction Report
        public void DTRForPrint(string DTRDate, int GodownId = 0)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRDate) });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("DailyTransactionReport2", CommandType.StoredProcedure, DParam);
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



        public void DTRForExport(string DTRDate, string DTRToDate, int GodownId = 0)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRToDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            try
            {
                DParam = LstParam.ToArray();
                DataSet ds = DataAccess.ExecuteDataSet("DailyTransactionExp", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                LNSM_DTRExp LNSM_obj = new LNSM_DTRExp();


                //get HDR Data


                if (ds.Tables.Count > 0)
                {
                    try
                    {

                        DataTable lstCartingDetails = ds.Tables[0];
                        DataTable lstShortCartingDetails = ds.Tables[1];
                        DataTable lstCargoShifting = ds.Tables[2];
                        DataTable lstCargoAccepting = ds.Tables[3];
                        DataTable lstBTTDetails = ds.Tables[4];
                        DataTable lstStuffingDetails = ds.Tables[5];

                        if (lstCartingDetails.Rows.Count > 0)
                        {
                            List<LNSM_CartingDetails> ListDtls1 = new List<LNSM_CartingDetails>();
                            LNSM_CartingDetails obj1 = new LNSM_CartingDetails();
                            obj1.NoOfPkg = lstCartingDetails.Rows[0]["NoOfPkg"].ToString();
                            obj1.ActualWeight = Convert.ToDecimal(lstCartingDetails.Rows[0]["ActualWeight"].ToString());
                            obj1.Area = Convert.ToDecimal(lstCartingDetails.Rows[0]["Area"].ToString());
                            ListDtls1.Add(obj1);
                            LNSM_obj.lstCartingDetails = ListDtls1;
                        }
                        if (lstShortCartingDetails.Rows.Count > 0)
                        {
                            List<LNSM_CartingDetails> ListDtls = new List<LNSM_CartingDetails>();
                            foreach (DataRow item in lstShortCartingDetails.Rows)
                            {
                                LNSM_CartingDetails obj2 = new LNSM_CartingDetails();
                                obj2.ShippingLineId = Convert.ToString(item["ShippingLineId"].ToString());
                                obj2.ShippingLineName = Convert.ToString(item["ShippingLineName"].ToString());
                                obj2.CFSCode = Convert.ToString(item["CFSCode"]);
                                obj2.ShippingBillNo = Convert.ToString(item["ShippingBillNo"].ToString());
                                obj2.ShippingBillDate = Convert.ToString(item["ShippingBillDate"].ToString());
                                obj2.ExporterName = Convert.ToString(item["ExporterName"].ToString());
                                obj2.CargoDescription = Convert.ToString(item["CargoDescription"].ToString());
                                obj2.ActualQty = Convert.ToDecimal(item["ActualQty"].ToString());
                                obj2.ActualWeight = Convert.ToDecimal(item["ActualWeight"].ToString());
                                obj2.FobValue = Convert.ToDecimal(item["FobValue"].ToString());
                                obj2.Slot = Convert.ToString(item["Slot"].ToString());
                                obj2.GR = Convert.ToString(item["GR"].ToString());
                                obj2.Area = Convert.ToDecimal(item["Area"].ToString());
                                obj2.Remarks = Convert.ToString(item["Remarks"].ToString());

                                ListDtls.Add(obj2);

                            }
                            //ObjTR70BLDTL.listBldtl = ListDtls;
                            LNSM_obj.lstShortCartingDetails = ListDtls;
                        }
                        if (lstCargoShifting.Rows.Count > 0)
                        {
                            List<LNSM_CargoAcceptingDetails> ListDtls = new List<LNSM_CargoAcceptingDetails>();
                            foreach (DataRow item in lstCargoShifting.Rows)
                            {
                                LNSM_CargoAcceptingDetails obj3 = new LNSM_CargoAcceptingDetails();
                                obj3.ShippingBillNo = Convert.ToString(item["ShippingBillNo"].ToString());
                                obj3.ShippingBillDate = Convert.ToString(item["ShippingBillDate"].ToString());
                                obj3.ExporterName = Convert.ToString(item["ExporterName"]);
                                obj3.CargoDescription = Convert.ToString(item["CargoDescription"].ToString());
                                obj3.ShippingBillDate = Convert.ToString(item["ShippingBillDate"].ToString());
                                obj3.ExporterName = Convert.ToString(item["ExporterName"].ToString());
                                obj3.ActualQty = Convert.ToDecimal(item["ActualQty"].ToString());
                                obj3.FobValue = Convert.ToDecimal(item["FobValue"].ToString());
                                obj3.Slot = Convert.ToString(item["Slot"].ToString());
                                obj3.GR = Convert.ToString(item["GR"].ToString());
                                obj3.Area = Convert.ToDecimal(item["Area"].ToString());
                                obj3.FromGodown = Convert.ToString(item["FromGodown"].ToString());
                                obj3.ToGodown = Convert.ToString(item["ToGodown"].ToString());
                                obj3.FromShippingLine = Convert.ToString(item["FromShippingLine"].ToString());
                                obj3.ToShippingLine = Convert.ToString(item["ToShippingLine"].ToString());

                                ListDtls.Add(obj3);

                            }
                            LNSM_obj.lstCargoShifting = ListDtls;
                        }
                        if (lstCargoAccepting.Rows.Count > 0)
                        {
                            List<LNSM_CargoAcceptingDetails> ListDtls = new List<LNSM_CargoAcceptingDetails>();
                            foreach (DataRow item in lstCargoAccepting.Rows)
                            {
                                LNSM_CargoAcceptingDetails obj4 = new LNSM_CargoAcceptingDetails();
                                obj4.CFSCode = Convert.ToString(item["CFSCode"].ToString());
                                obj4.ShippingBillNo = Convert.ToString(item["ShippingBillNo"].ToString());
                                obj4.ShippingBillDate = Convert.ToString(item["ShippingBillDate"]);
                                obj4.ExporterName = Convert.ToString(item["ExporterName"].ToString());
                                obj4.FobValue = Convert.ToDecimal(item["FobValue"].ToString());
                                obj4.ActualWeight = Convert.ToDecimal(item["ActualWeight"].ToString());
                                obj4.CargoDescription = Convert.ToString(item["CargoDescription"].ToString());
                                obj4.ActualQty = Convert.ToDecimal(item["ActualQty"].ToString());
                                obj4.Slot = Convert.ToString(item["Slot"].ToString());
                                obj4.GR = Convert.ToString(item["GR"].ToString());
                                obj4.Area = Convert.ToDecimal(item["Area"].ToString());
                                obj4.FromGodown = Convert.ToString(item["FromGodown"].ToString());
                                obj4.ToGodown = Convert.ToString(item["ToGodown"].ToString());
                                obj4.FromShippingLine = Convert.ToString(item["FromShippingLine"].ToString());
                                obj4.ToShippingLine = Convert.ToString(item["ToShippingLine"].ToString());

                                ListDtls.Add(obj4);


                            }
                            LNSM_obj.lstCargoAccepting = ListDtls;

                        }
                        if (lstBTTDetails.Rows.Count > 0)
                        {
                            List<LNSM_BTTDetails> ListDtls = new List<LNSM_BTTDetails>();
                            foreach (DataRow item in lstBTTDetails.Rows)
                            {
                                LNSM_BTTDetails obj5 = new LNSM_BTTDetails();
                                obj5.CFSCode = Convert.ToString(item["CFSCode"].ToString());
                                obj5.ShippingBillNo = Convert.ToString(item["ShippingBillNo"].ToString());
                                obj5.ShippingBillDate = Convert.ToString(item["ShippingBillDate"]);
                                obj5.ExporterName = Convert.ToString(item["ExporterName"].ToString());
                                obj5.Fob = Convert.ToDecimal(item["Fob"].ToString());

                                obj5.CargoDescription = Convert.ToString(item["CargoDescription"].ToString());
                                obj5.LocationName = Convert.ToString(item["LocationName"].ToString());
                                obj5.BTTQuantity = Convert.ToDecimal(item["BTTQuantity"].ToString());
                                obj5.BTTWeight = Convert.ToDecimal(item["BTTWeight"].ToString());
                                obj5.Area = Convert.ToDecimal(item["Area"].ToString());


                                ListDtls.Add(obj5);


                            }
                            LNSM_obj.lstBTTDetails = ListDtls;

                        }
                        if (lstStuffingDetails.Rows.Count > 0)
                        {
                            List<LNSM_StuffingDetails> ListDtls = new List<LNSM_StuffingDetails>();
                            foreach (DataRow item in lstStuffingDetails.Rows)
                            {
                                LNSM_StuffingDetails obj6 = new LNSM_StuffingDetails();
                                obj6.StuffingNo = Convert.ToString(item["StuffingNo"].ToString());
                                obj6.StuffingDate = Convert.ToString(item["StuffingDate"].ToString());
                                obj6.CFSCode = Convert.ToString(item["CFSCode"]);
                                obj6.ContainerNo = Convert.ToString(item["ContainerNo"].ToString());
                                obj6.Size = Convert.ToString(item["Size"].ToString());
                                obj6.SLA = Convert.ToString(item["SLA"].ToString());
                                obj6.EntryDateTime = Convert.ToString(item["EntryDateTime"].ToString());
                                obj6.StuffQuantity = Convert.ToDecimal(item["StuffQuantity"].ToString());
                                obj6.StuffWeight = Convert.ToDecimal(item["StuffWeight"].ToString());
                                obj6.Fob = Convert.ToDecimal(item["FOB"].ToString());


                                ListDtls.Add(obj6);


                            }
                            LNSM_obj.lstStuffingDetails = ListDtls;


                        }
                        Status = 1;
                    }
                    catch (Exception EX)
                    {

                    }
                }
                else
                {
                    Status = 0;
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LNSM_obj;
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

            //Result = DataAccess.ExecuteDataSet("DailyTransactionExp", CommandType.StoredProcedure, DParam);

            //_DBResponse = new DatabaseResponse();
            //if (Result != null && Result.Tables[0].Rows.Count > 0)
            //{
            //    Status = 1;
            //}
            //else if (Result != null && Result.Tables[1].Rows.Count > 0)
            //{
            //    Status = 1;
            //}
        }
        //public void DTRForExport(string DTRDate, string DTRToDate, int GodownId = 0)
        //{
        //    IDataParameter[] DParam = { };
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    //LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRDate) });
        //    //LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRToDate) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        DParam = LstParam.ToArray();
        //        //Result = DataAccess.ExecuteDataSet("DailyTransactionExp", CommandType.StoredProcedure, DParam);
        //        LNSM_DTRExp obj = new LNSM_DTRExp();
        //        obj = (LNSM_DTRExp)DataAccess.ExecuteDynamicSet<LNSM_DTRExp>("DailyTransactionExp", DParam);
        //        if (obj.lstBTTDetails.Count > 0 || obj.lstCargoAccepting.Count > 0 || obj.lstCargoShifting.Count > 0 || obj.lstCartingDetails.Count > 0 ||
        //            obj.lstStuffingDetails.Count > 0 ||  obj.lstShortCartingDetails.Count > 0)
        //            _DBResponse.Status = 1;
        //        else
        //            _DBResponse.Status = 0;
        //        _DBResponse.Message = "Data";
        //        _DBResponse.Data = obj;
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Data = null;
        //    }
        //}
        public void DTRForImportFCL(string DTRDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "As_On", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRDate) });
            _DBResponse = new DatabaseResponse();
            try
            {
                DParam = LstParam.ToArray();
                DataSet Result = DataAccess.ExecuteDataSet("GetImpDTRFCL", CommandType.StoredProcedure, DParam);
                if (Result.Tables[0].Rows.Count > 0)
                    _DBResponse.Status = 1;
                else
                    _DBResponse.Status = 0;
                _DBResponse.Message = "Data";
                _DBResponse.Data = Result;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        #endregion

        #region Get All Godown
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
            List<LNSM_Godown> LstGodown = new List<LNSM_Godown>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstGodown.Add(new LNSM_Godown
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Result["GodownName"].ToString()
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

        #endregion
        #region Consol Register of Outward Supply
        public void ConsolGetRegisterofOutwardSupply(DateTime date1, DateTime date2)
        {
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            DataSet ds = DataAccess.ExecuteDataSet("GetConsRegisterofOutwardSupply", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            
                List<LNSM_ConslRegisterOfOutwardSupplyModel> model = new List<LNSM_ConslRegisterOfOutwardSupplyModel>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        model = (from DataRow dr in dt.Rows
                                 select new LNSM_ConslRegisterOfOutwardSupplyModel()
                                 {
                                     //SlNo = Convert.ToInt32(dr["SlNo"]),
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

                                     PaymentMode = dr["PaymentMode"].ToString(),
                                     Remarks = dr["Remarks"].ToString()
                                     ,
                                     HSNCode = dr["HSNCode"].ToString()
                                 }).ToList();
                    }
                    decimal InvoiceAmount = 0, CRAmount = 0;

                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = ConslRegisterofOutwardSupplyExcel(model, InvoiceAmount, CRAmount);
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


        #endregion

        #region Consol Register of Outward Supply Excel
        private string ConslRegisterofOutwardSupplyExcel(List<LNSM_ConslRegisterOfOutwardSupplyModel> model, decimal InvoiceAmount, decimal CRAmount)
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
                exl.MargeCell("R2:R4", "Total Invoice Value" + Environment.NewLine + "(18=(11+13 or 11+15+17))", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("S2:S4", "PaymentMode", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T2:T4", "Remarks", DynamicExcel.CellAlignment.Middle);


                for (var i = 65; i < 85; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                }

                exl.AddTable<LNSM_ConslRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 10, 30 });
                var igstamt = model.Sum(o => o.ITaxAmount);
                var sgstamt = model.Sum(o => o.STaxAmount);
                var cgstamt = model.Sum(o => o.CTaxAmount);
                var totalamt = model.Sum(o => o.Total);
                exl.AddCell("M" + (model.Count + 6).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 6).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (model.Count + 6).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("R" + (model.Count + 6).ToString(), totalamt.ToString(), DynamicExcel.CellAlignment.CenterRight);


                exl.Save();
            }
            return excelFile;
        }
        #endregion


        #region Ageing
        public void GetAllPartyForAgeing(string PartyCode, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyForAgeing", CommandType.StoredProcedure, DParam);
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
        public void AgeingStatement(string AgeingDate, int PartyId)
        {

            DateTime dtfrom = DateTime.ParseExact(AgeingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            // DateTime dtTo = DateTime.ParseExact(ObjCargoInStockReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status
            _DBResponse = new DatabaseResponse();

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Ageing", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            DataSet ds = DataAccess.ExecuteDataSet("GetAgeingStatement", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            try
            {


                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = ds;// AgeingDetailsExcel(dt, AgeingDate);
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

        private string AgeingDetailsExcel( DataTable dt, string datevalue)
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


                    if (i % 2 != 0)
                    {
                        Grid.Rows[i].Attributes.Add("class", "textmode");
                    }
                    else
                    {
                        Grid.Rows[i].Attributes.Add("class", "textmode2");
                    }

                }
                var title ="Ageing As On Date" + datevalue + "</br>";

                System.Web.UI.WebControls.TableCell cell1 = new System.Web.UI.WebControls.TableCell();
            //    cell1.Text = "<b> CENTRAL WAREHOUSING CORPORATION </b> ";
                cell1.ForeColor = System.Drawing.Color.Black;
                System.Web.UI.WebControls.TableRow tr1 = new System.Web.UI.WebControls.TableRow();
                tr1.Cells.Add(cell1);
                tr1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                //System.Web.UI.WebControls.TableCell cell2 = new System.Web.UI.WebControls.TableCell();
                //cell2.Text = "Principal Place of Business";
                //System.Web.UI.WebControls.TableRow tr2 = new System.Web.UI.WebControls.TableRow();
                //tr2.Cells.Add(cell2);
                //tr2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                System.Web.UI.WebControls.TableCell cell3 = new System.Web.UI.WebControls.TableCell();
               // cell3.Text = "CENTRAL WAREHOUSE";
                cell3.ForeColor = System.Drawing.Color.Black;

                System.Web.UI.WebControls.TableRow tr3 = new System.Web.UI.WebControls.TableRow();
                tr3.Cells.Add(cell3);
                tr3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;


                System.Web.UI.WebControls.TableCell cell4 = new System.Web.UI.WebControls.TableCell();
           //     cell4.Text = "Consolidate Party Ledger Statement As On Date " + datevalue + "";
                System.Web.UI.WebControls.TableRow tr4 = new System.Web.UI.WebControls.TableRow();
                cell4.ForeColor = System.Drawing.Color.Black;

                tr4.Cells.Add(cell4);
                tr4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;



                System.Web.UI.WebControls.TableCell cell5 = new System.Web.UI.WebControls.TableCell();
                cell5.Controls.Add(Grid);
                System.Web.UI.WebControls.TableRow tr5 = new System.Web.UI.WebControls.TableRow();
                tr5.Cells.Add(cell5);

                tb.Rows.Add(tr1);
                //  tb.Rows.Add(tr2);
                tb.Rows.Add(tr3);
                tb.Rows.Add(tr4);
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


        #region Credit Note 
        public void GetAllCreditNote(int Page, string PeriodFrom, string PeriodTo, string invNo, string PartyGSTNo, string PartyName, string RefInvoiceNo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                var LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.VarChar, Value = Page });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodFrom).ToString("yyyy-MM-dd") });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodTo).ToString("yyyy-MM-dd") });
                LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = invNo });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PartyGSTNo", MySqlDbType = MySqlDbType.VarChar, Value = PartyGSTNo });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = PartyName });
                LstParam.Add(new MySqlParameter { ParameterName = "in_RefInvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = RefInvoiceNo });
             

                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                _DBResponse = new DatabaseResponse();
                Result = DataAccess.ExecuteDataSet("GetAllCreditNote", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<LNSM_CreditNote> InvcList = new List<LNSM_CreditNote>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        LNSM_CreditNote objInv = new LNSM_CreditNote();
                        objInv.InvoiceId = Convert.ToInt32(dr["InvoiceId"]);
                        objInv.InvoiceNumber = Convert.ToString(dr["InvoiceNo"]);
                        objInv.CRNoteNo = Convert.ToString(dr["CRNoteNo"]);
                        objInv.RefInvoiceNo = Convert.ToString(dr["RefCRNoteNo"]);
                        objInv.InvoiceDate = Convert.ToString(dr["CRNoteDate"]);
                        objInv.PartyGSTNo = Convert.ToString(dr["PartyGSTNo"]);
                        objInv.PartyName = Convert.ToString(dr["PartyName"]);
                        objInv.TotalAmt = Convert.ToDecimal(dr["TotalAmt"]);
                        InvcList.Add(objInv);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = InvcList;
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


        public void PrintPDFForCRNote(string CRNoteId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRNoteId", MySqlDbType = MySqlDbType.Int32, Value =Convert.ToInt32(CRNoteId) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintPDFOfCRNote", CommandType.StoredProcedure, DParam);
            PrintPDFModelOfCr objCR = new PrintPDFModelOfCr();
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

                    objCR.ver = Convert.ToString(Result["ver"]);
                    objCR.pa = Convert.ToString(Result["pa"]);
                    //  objCR.tr = Convert.ToString(Result["tr"]);
                    objCR.tn = Convert.ToString(Result["tn"]);
                    objCR.tier = Convert.ToString(Result["tier"]);
                    objCR.tid = Convert.ToString(Result["tid"]);
                    objCR.qrMedium = Convert.ToInt32(Result["qrMedium"]);

                    objCR.QRexpire = Convert.ToString(Result["QRexpireDays"]);
                    objCR.pn = Convert.ToString(Result["pn"]);
                    objCR.pinCode = Convert.ToInt32(Result["PinCode"]);
                    objCR.orgId = Convert.ToInt32(Result["orgId"]);
                    objCR.mtid = Convert.ToString(Result["mtid"]);
                    objCR.msid = Convert.ToString(Result["msid"]);
                    objCR.mode = Convert.ToInt32(Result["mode"]);
                    objCR.mid = Convert.ToString(Result["mid"]);
                    objCR.mc = Convert.ToString(Result["mc"]);
                    objCR.gstIn = Convert.ToString(Result["GstIn"]);
                   



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

                        objCR.irn = Result["irn"].ToString();
                        objCR.SignedQRCode = Result["SignedQRCode"].ToString();
                        objCR.SupplyType = Result["SupplyType"].ToString();

                        // objCR.QRInvoiceDate = Convert.ToString(Result["pa"]);
                        objCR.CGST = Convert.ToDecimal(Result["CGST"]);
                        objCR.CESS = 0;
                        objCR.am = Convert.ToDecimal(Result["InvoiceAmt"]);
                        objCR.mam = Convert.ToDecimal(Result["InvoiceAmt"]);
                        objCR.IGST = Convert.ToDecimal(Result["IGST"]);
                        objCR.GSTPCT = Convert.ToDecimal(Result["IGSTPer"]);
                        objCR.QRInvoiceDate = Convert.ToString(Result["DocDt"]);
                        objCR.SGST = Convert.ToDecimal(Result["SGST"]);
                        objCR.CGST = Convert.ToDecimal(Result["CGST"]);
                        objCR.tr = Convert.ToString(Result["CRNoteId"]);
                        objCR.AmountInWord = Convert.ToString(Result["GrossAmountInWard"]);

                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objCR.lstCharges.Add(new ChargesModels
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


        #endregion

        #region Bulk EInvoice Generation

        public void GetBulkIrnDetails()
        {
            int Status = 0;

            IDataParameter[] DParam = { };

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            LNSM_BulkIRN objInvoice = new LNSM_BulkIRN();
            IDataReader Result = DataAccess.ExecuteDataReader("IRNNotgeneratedInvoiceList", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    objInvoice.lstPostPaymentChrg.Add(new LNSM_BulkIRNDetails
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

        public void GetIRNForYard(String InvoiceNo)
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

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIRNDetails", CommandType.StoredProcedure, DParam);
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
                        seller.TrdNm = Result["TrdNm"].ToString();
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
                        buyer.TrdNm = Result["TrdNm"].ToString();
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
                        ship.TrdNm = Result["TrdNm"].ToString();
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
                            PrdDesc = null,//Result["PrdDesc"].ToString(),
                            IsServc = Result["IsServc"].ToString(),
                            HsnCd = Result["HsnCd"].ToString(),
                            Barcde = Result["Barcde"].ToString(),
                            Qty = Convert.ToDecimal(Result["Qty"].ToString()),
                            FreeQty = Convert.ToInt32(Result["FreeQty"].ToString()),
                            Unit = null, // Result["Unit"].ToString(),
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
        public void GetIRNB2CForYard(String InvoiceNo)
        {
            LNSM_IrnB2CDetails Obj = new LNSM_IrnB2CDetails();

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIrnB2CDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Obj.DocNo = Convert.ToString(Result["DocNo"]);
                    Obj.DocDt = Convert.ToString(Result["DocDt"]);
                    Obj.DocTyp = Convert.ToString(Result["DocTyp"]);
                    Obj.BuyerGstin = Convert.ToString(Result["BuyerGstin"]);
                    Obj.SellerGstin = Convert.ToString(Result["SellerGST"]);

                    Obj.MainHsnCode = Convert.ToString(Result["MainHsnCode"]);
                    Obj.TotInvVal = Convert.ToInt32(Result["TotInvVal"]);
                    Obj.ItemCnt = Convert.ToInt32(Result["ItemCnt"]);
                    Obj.iss = null;
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

        public void AddEditIRNResponsec(IrnResponse objOBL, string InvoiceNo)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
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

            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("Addeditirnresponce", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "IRN Generate Successfully" : "IRN Generate Successfully";
                    _DBResponse.Status = Result;
                }

                else
                {
                   // log.Info("After Calling Stored Procedure Error" + " Invoice No " + InvoiceNo + " signed Invoice: " + objOBL.SignedInvoice + " SignedQRCode " + objOBL.SignedQRCode);
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


        public void GetHeaderIRNForYard()
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
                            PrdDesc = null,//Result["PrdDesc"].ToString(),
                            IsServc = Result["IsServc"].ToString(),
                            HsnCd = Result["HsnCd"].ToString(),
                            Barcde = Result["Barcde"].ToString(),
                            Qty = Convert.ToDecimal(Result["Qty"].ToString()),
                            FreeQty = Convert.ToInt32(Result["FreeQty"].ToString()),
                            Unit = null,// Result["Unit"].ToString(),
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

        public void GetIRNForB2CInvoiceForCreditDebitNoteDN(String CrNoteNo, String TypeOfInv, String CRDR)
        {
            LNSM_IrnB2CDetails Obj = new LNSM_IrnB2CDetails();

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

                    //log.Info("After Calling Stored Procedure Error" + " Credit Note No " + CrNoteNo + " signed Invoice: " + objOBL.SignedInvoice + " SignedQRCode " + objOBL.SignedQRCode);

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

        #region Get IRN Response
        public void GetIRNResponse(string PeriodFrom, string PeriodTo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                var LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodFrom).ToString("yyyy-MM-dd") });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodTo).ToString("yyyy-MM-dd") });


                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                _DBResponse = new DatabaseResponse();
                Result = DataAccess.ExecuteDataSet("GetIRNResponse", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<IRNResponseModel> InvcList = new List<IRNResponseModel>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        IRNResponseModel objInv = new IRNResponseModel();
                        objInv.IRNDate = (dr["AckDt"].ToString());
                        objInv.InvoiceNumber = Convert.ToString(dr["InvoiceNo"]);
                        objInv.IRNResponse = Convert.ToString(dr["irnresponse"]);
                        objInv.IRNRefNo = Convert.ToString(dr["IRNRefNo"]);
                        objInv.ErrorMessage = Convert.ToString(dr["ErrorMessage"]);
                   
                        InvcList.Add(objInv);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = InvcList;
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



        #region JSON Response Status
        public void GetJSONResponse(string PeriodFrom, string PeriodTo, string IntType, string StatusM)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {

                var LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_INTType", MySqlDbType = MySqlDbType.VarChar, Value = IntType.ToString() });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodFrom).ToString("yyyy-MM-dd") });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodTo).ToString("yyyy-MM-dd") });
     
                LstParam.Add(new MySqlParameter { ParameterName = "in_Status", MySqlDbType = MySqlDbType.VarChar, Value = StatusM.ToString() });


                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                _DBResponse = new DatabaseResponse();
                Result = DataAccess.ExecuteDataSet("GetJsonResponseStatus", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<LNSM_JsonRespnStatusModel> InvcList = new List<LNSM_JsonRespnStatusModel>();

                    if(IntType== "IRN")
                    {
                    if (Result != null && Result.Tables.Count > 0)
                    {
                        foreach (DataRow dr in Result.Tables[0].Rows)
                        {
                            Status = 1;
                            LNSM_JsonRespnStatusModel objInv = new LNSM_JsonRespnStatusModel();
                            objInv.InvoiceId = Convert.ToInt32(dr["InvoiceId"]);
                            objInv.InvoiceNumber = (dr["InvoiceNo"].ToString());
                            objInv.InvoiceDate = Convert.ToString(dr["InvoiceDate"]);
                            objInv.Message = Convert.ToString(dr["StatusMessage"]);
                            objInv.jsonEInvoice = Convert.ToString(dr["jsonEInvoice"]);
                       

                            InvcList.Add(objInv);
                        }
                    }

                }
                else
                    {
                    if (Result != null && Result.Tables.Count > 0)
                    {
                        foreach (DataRow dr in Result.Tables[0].Rows)
                        {
                            Status = 1;

                            LNSM_JsonRespnStatusModel objInv = new LNSM_JsonRespnStatusModel();
                            objInv.InvoiceId =Convert.ToInt32(dr["CashReceiptId"].ToString());
                            objInv.InvoiceNumber = (dr["ReceiptNo"].ToString());
                            objInv.InvoiceDate = Convert.ToString(dr["ReceiptDate"]);
                            objInv.Message = Convert.ToString(dr["StatusMessage"]);

                            InvcList.Add(objInv);
                        }
                    }

                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = InvcList;
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
        public void GetJsonString(string InvoiceId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {

                var LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceId });
     


                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                _DBResponse = new DatabaseResponse();
                Result = DataAccess.ExecuteDataSet("GetJsonString", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<LNSM_JsonRespnStatusModel> InvcList = new List<LNSM_JsonRespnStatusModel>();

                    if (Result != null && Result.Tables.Count > 0)
                    {
                        foreach (DataRow dr in Result.Tables[0].Rows)
                        {
                            Status = 1;
                            LNSM_JsonRespnStatusModel objInv = new LNSM_JsonRespnStatusModel();
                            objInv.InvoiceId = Convert.ToInt32(dr["InvoiceId"]);                        
                            objInv.jsonEInvoice = Convert.ToString(dr["jsonEInvoice"]);
                       

                            InvcList.Add(objInv);
                        }
                    }          

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = InvcList;
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
        public void GetJsonResponseStringFile(string InvoiceId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {

                var LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceId });
     


                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                _DBResponse = new DatabaseResponse();
                Result = DataAccess.ExecuteDataSet("GetJsonResponse", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<LNSM_JsonRespnStatusModel> InvcList = new List<LNSM_JsonRespnStatusModel>();

                    if (Result != null && Result.Tables.Count > 0)
                    {
                        foreach (DataRow dr in Result.Tables[0].Rows)
                        {
                            Status = 1;
                            LNSM_JsonRespnStatusModel objInv = new LNSM_JsonRespnStatusModel();
                            objInv.InvoiceId = Convert.ToInt32(dr["InvoiceId"]);                        
                            objInv.InvoiceNumber = (dr["InvoiceNo"].ToString());                        
                            objInv.StatusCode = (dr["StatusCode"].ToString());                        
                            objInv.Message =(dr["StatusMessage"].ToString());                        
                            objInv.SendStatus = (dr["SendStatus"].ToString());
                       

                            InvcList.Add(objInv);
                        }
                    }          

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = InvcList;
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
        public void GetReciptJsonString(string InvoiceId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {

                var LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value =Convert.ToInt32(InvoiceId)});



                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                _DBResponse = new DatabaseResponse();
                Result = DataAccess.ExecuteDataSet("GetCashReciptJsonString", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<LNSM_JsonRespnStatusModel> InvcList = new List<LNSM_JsonRespnStatusModel>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        LNSM_JsonRespnStatusModel objInv = new LNSM_JsonRespnStatusModel();
                        objInv.InvoiceId = Convert.ToInt32(dr["CashReceiptId"]);
                        objInv.InvoiceNumber = (dr["CashReceiptNo"].ToString()); 
                        objInv.jsonEInvoice = Convert.ToString(dr["jsonEInvoice"]);


                        InvcList.Add(objInv);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = InvcList;
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


        public void GetJsonResponseCashReciptStringFile(string InvoiceId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {

                var LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.VarChar, Value =Convert.ToInt32(InvoiceId) });



                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                _DBResponse = new DatabaseResponse();
                Result = DataAccess.ExecuteDataSet("GetJsonResponseCashRecipt", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<LNSM_JsonRespnStatusModel> InvcList = new List<LNSM_JsonRespnStatusModel>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        LNSM_JsonRespnStatusModel objInv = new LNSM_JsonRespnStatusModel();
                        objInv.InvoiceId = Convert.ToInt32(dr["CashReceiptId"]);
                        objInv.InvoiceNumber = (dr["CashReceiptNo"].ToString());
                        objInv.StatusCode = (dr["StatusCode"].ToString());
                        objInv.Message = (dr["StatusMessage"].ToString());
                     


                        InvcList.Add(objInv);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = InvcList;
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

        #region Bulk Invoice 
        public void BulkInvoiceDetailsForPrint(LNSM_InvoiceInformation ObjInvoice)
        {

            DateTime dtTo = DateTime.ParseExact(ObjInvoice.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            DateTime dtfrom = DateTime.ParseExact(ObjInvoice.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");



            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInvoice.InvoiceNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjInvoice.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjInvoice.InvoiceModule });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom  });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });



            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBulkInvoicePrint", CommandType.StoredProcedure, DParam);
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


        #region DTR Rail Summary
        public void DTRRailSummary(LNSM_DTRRailSummary ObjDailyCashBook)
        {
            var LstParam = new List<MySqlParameter>();
  
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDailyCashBook.PeriodFrom).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDailyCashBook.PeriodTo).ToString("yyyy-MM-dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            DataSet ds = DataAccess.ExecuteDataSet("DailyTransactionReportRailSum", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];

            List<LNSM_DTRRailSummary> model = new List<LNSM_DTRRailSummary>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    model = (from DataRow dr in dt.Rows
                             select new LNSM_DTRRailSummary()
                             {
                                 SlNo = Convert.ToInt32(dr["SlNo"]),
                                 Movementdate =(dr["Movementdate"].ToString()),
                                 TrainNumber = dr["TrainNumber"].ToString(),
                                 FromLocation = dr["FromLocation"].ToString(),
                                 ToLocation = dr["ToLocation"].ToString(),

                                 ContType = dr["ContType"].ToString(),
                                 WagonNo = (dr["WagonNo"].ToString()),
                                 ContNumber = dr["ContNumber"].ToString(),
                                 ContStatus = dr["ContStatus"].ToString(),
                                 Size =(dr["Size"].ToString()),

                                 Cargo = dr["Cargo"].ToString(),
                                 CargoWt = (dr["CargoWt"].ToString()),
                                 TareWt = dr["TareWt"].ToString(),
                                 OperatonType = (dr["OperatonType"].ToString()),
                                 TotalWt = (dr["TotalWt"].ToString())
                               
                             }).ToList();
                }
                decimal InvoiceAmount = 0, CRAmount = 0;

                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = DTRRailSummaryExcel2(model, ObjDailyCashBook.PeriodFrom.ToString(), ObjDailyCashBook.PeriodTo.ToString());
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

        private string DTRRailSummaryExcel(List<LNSM_DTRRailSummary> model)
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
                         + "Daily Transaction Rail Summary ";
                exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("N1:O1", "BILL REGISTER", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:A4", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B2:B4", "Movementdate", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C2:C4", "TrainNumber" , DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D2:D4", "FromLocation", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E2:E4", "ToLocation", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F2:F4", "ContType" , DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G2:G4", "WagonNo", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H2:H4", "ContNumber", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I2:I4", "ContStatus", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J2:J4", "Size", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K2:K4", "Cargo", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L2:L4", "CargoWt", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M2:M4", "TareWt", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N2:N4", "OperatonType", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O2:O4", "TotalWt", DynamicExcel.CellAlignment.Middle);


                //for (var i = 65; i < 85; i++)
                //{
                //    char character = (char)i; 
                //    string text = character.ToString();
                //    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                //}

                exl.AddTable<LNSM_DTRRailSummary>("A", 5, model, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 10, 30 });
                //var igstamt = model.Sum(o => o.ITaxAmount);
                //var sgstamt = model.Sum(o => o.STaxAmount);
                //var cgstamt = model.Sum(o => o.CTaxAmount);
                //var totalamt = model.Sum(o => o.Total);
                //exl.AddCell("M" + (model.Count + 6).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                //exl.AddCell("O" + (model.Count + 6).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                //exl.AddCell("Q" + (model.Count + 6).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                //exl.AddCell("R" + (model.Count + 6).ToString(), totalamt.ToString(), DynamicExcel.CellAlignment.CenterRight);


                exl.Save();
            }
            return excelFile;
        }


        //private string DTRRailSummaryExcel2(IList<LNSM_DTRRailSummary> OBJDailyCashBookPpg, string date1, string date2)
        private string DTRRailSummaryExcel2(List<LNSM_DTRRailSummary> OBJDailyCashBookPpg, string date1, string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Govt. of India Undertaking)"
                        + Environment.NewLine + "ICD Patparganj-Delhi"
                        + Environment.NewLine + Environment.NewLine
                        + "ExportContainer Income Detail";
                string typeOfValue = "";

                typeOfValue = "During Period Of " + date1 + " TO " + date2;



                exl.MargeCell("A1:H1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A2:H2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A3:H3", HttpContext.Current.Session["CompanayName"].ToString(), DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A4:H4", "Daily Transaction Rail Summary  " + typeOfValue, DynamicExcel.CellAlignment.Middle);
                /// exl.MargeCell("A5:H5", typeOfValue, DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("A4:F4", "DETAILS OF ORIGINAL INOICE", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("G4:O4", "DETAILS OF CREDIT NOTE ISSUED", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:A6", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B5:B6", "Movementdate", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C6", "TrainNumber", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D6", "FromLocation", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E6", "ToLocation.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F5:F6", "ContType", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G5:G6", "WagonNo", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("H5:H6", "ContNumber", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I5:I6", "ContStatus", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J5:J6", "Size", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K5:K6", "Cargo", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L5:L6", "CargoWt", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M5:M6", "TareWt", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N5:N6", "OperatonType", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O5:O6", "TotalWt", DynamicExcel.CellAlignment.Middle);




                exl.AddTable("A", 7, OBJDailyCashBookPpg, new[] { 6, 20, 20, 30, 12, 20, 20, 15, 15, 30, 15, 20, 20, 20, 15, 15, 15, 10, 12, 18, 18, 16, 12, 12, 15, 15, 17, 18, 15, 14, 15, 16, 16, 15, 15, 15, 15, 15, 15 });





                //var Total = PPGConIncomeDetail.Sum(o => o.Total);


                //var BOEValueDuty = PPGAssesmentSheetfcl.Sum(o => o.BOEValueDuty);



                ///exl.AddCell("AM" + (PPGConIncomeDetail.Count + 7).ToString(), TaxAmt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("AM" + (PPGConIncomeDetail.Count + 7).ToString(),Total.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.Save();
            }
            return excelFile;
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
            List<LNSM_OutstandingAmountReport> lstEmptyContDtl = new List<LNSM_OutstandingAmountReport>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstEmptyContDtl.Add(new LNSM_OutstandingAmountReport
                    {

                        BillingNo = Result["InvoiceNo"].ToString(),
                        BillingDate = Result["InvoiceDate"].ToString(),
                        //SLNo =Convert.ToInt32(Result["SlNo"]),
                        AmountReceivalbe = Convert.ToDecimal(Result["GrossTotal"].ToString()),
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

        #region Bulk Invoice User
        public void BulkInvoiceDetailsForPrintUser(LNSM_InvoiceInformation ObjInvoice)
        {

            DateTime dtTo = DateTime.ParseExact(ObjInvoice.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            DateTime dtfrom = DateTime.ParseExact(ObjInvoice.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");



            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInvoice.InvoiceNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjInvoice.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjInvoice.InvoiceModule });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });



            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBulkInvoicePrintUser", CommandType.StoredProcedure, DParam);
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


        #region Credit Note 
        public void GetAllDebitNote(int Page, string PeriodFrom, string PeriodTo, string invNo, string PartyGSTNo, string PartyName, string RefInvoiceNo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                var LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.VarChar, Value = Page });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodFrom).ToString("yyyy-MM-dd") });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodTo).ToString("yyyy-MM-dd") });
                LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = invNo });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PartyGSTNo", MySqlDbType = MySqlDbType.VarChar, Value = PartyGSTNo });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = PartyName });
                LstParam.Add(new MySqlParameter { ParameterName = "in_RefInvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = RefInvoiceNo });


                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                _DBResponse = new DatabaseResponse();
                Result = DataAccess.ExecuteDataSet("GetAllDebitNote", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<LNSM_CreditNote> InvcList = new List<LNSM_CreditNote>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        LNSM_CreditNote objInv = new LNSM_CreditNote();
                        objInv.InvoiceId = Convert.ToInt32(dr["InvoiceId"]);
                        objInv.InvoiceNumber = Convert.ToString(dr["InvoiceNo"]);
                        objInv.CRNoteNo = Convert.ToString(dr["CRNoteNo"]);
                        objInv.RefInvoiceNo = Convert.ToString(dr["RefCRNoteNo"]);
                        objInv.InvoiceDate = Convert.ToString(dr["CRNoteDate"]);
                        objInv.PartyGSTNo = Convert.ToString(dr["PartyGSTNo"]);
                        objInv.PartyName = Convert.ToString(dr["PartyName"]);
                        objInv.TotalAmt = Convert.ToDecimal(dr["TotalAmt"]);
                        InvcList.Add(objInv);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = InvcList;
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


        public void PrintPDFForDebitNote(string CRNoteId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRNoteId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(CRNoteId) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintPDFOfDRNote", CommandType.StoredProcedure, DParam);
            PrintPDFModelOfCr objCR = new PrintPDFModelOfCr();
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

                    objCR.ver = Convert.ToString(Result["ver"]);
                    objCR.pa = Convert.ToString(Result["pa"]);
                    //  objCR.tr = Convert.ToString(Result["tr"]);
                    objCR.tn = Convert.ToString(Result["tn"]);
                    objCR.tier = Convert.ToString(Result["tier"]);
                    objCR.tid = Convert.ToString(Result["tid"]);
                    objCR.qrMedium = Convert.ToInt32(Result["qrMedium"]);

                    objCR.QRexpire = Convert.ToString(Result["QRexpireDays"]);
                    objCR.pn = Convert.ToString(Result["pn"]);
                    objCR.pinCode = Convert.ToInt32(Result["PinCode"]);
                    objCR.orgId = Convert.ToInt32(Result["orgId"]);
                    objCR.mtid = Convert.ToString(Result["mtid"]);
                    objCR.msid = Convert.ToString(Result["msid"]);
                    objCR.mode = Convert.ToInt32(Result["mode"]);
                    objCR.mid = Convert.ToString(Result["mid"]);
                    objCR.mc = Convert.ToString(Result["mc"]);
                    objCR.gstIn = Convert.ToString(Result["GstIn"]);



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

                        objCR.irn = Result["irn"].ToString();
                        objCR.SignedQRCode = Result["SignedQRCode"].ToString();
                        objCR.SupplyType = Result["SupplyType"].ToString();

                        // objCR.QRInvoiceDate = Convert.ToString(Result["pa"]);
                        objCR.CGST = Convert.ToDecimal(Result["CGST"]);
                        objCR.CESS = 0;
                        objCR.am = Convert.ToDecimal(Result["InvoiceAmt"]);
                        objCR.mam = Convert.ToDecimal(Result["InvoiceAmt"]);
                        objCR.IGST = Convert.ToDecimal(Result["IGST"]);
                        objCR.GSTPCT = Convert.ToDecimal(Result["IGSTPer"]);
                        objCR.QRInvoiceDate = Convert.ToString(Result["DocDt"]);
                        objCR.SGST = Convert.ToDecimal(Result["SGST"]);
                        objCR.CGST = Convert.ToDecimal(Result["CGST"]);
                        objCR.tr = Convert.ToString(Result["CRNoteId"]);


                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objCR.lstCharges.Add(new ChargesModels
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


        #endregion

        #region TDS Report

        public void GetEximTraderForPartyLedger()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPayeeForTDS", CommandType.StoredProcedure, DParam);
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

        public void TdsReport(LNSM_TDSReport ObjTDSReport)
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
            LNSM_TDSMain objTDSMain = new LNSM_TDSMain();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;                    

                    objTDSMain.TDSReportLst.Add(new LNSM_TDSReport
                    {
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        CRNo = Result["ReceiptNo"].ToString(),
                        CRDate = Result["ReceiptDate"].ToString(),
                        PartyTAN = Result["TanNumber"].ToString(),
                        InvoiceValue = Result["InvoiceValue"].ToString(),
                        TDS = Result["TDS"].ToString(),
                        TDSPlus = Result["TDSPlus"].ToString(),
                        Amount = Result["Amount"].ToString(),
                        ReceiptAmount = Result["ReceiptAmt"].ToString(),

                    });
                }
                if (Status == 1)
                {
                    objTDSMain.TDSReportLst.Add(new LNSM_TDSReport
                    {
                        InvoiceNo = "<strong>Total</strong>",
                        InvoiceDate = string.Empty,
                        CRNo = string.Empty,
                        CRDate = string.Empty,
                        PartyTAN = string.Empty,
                        InvoiceValue = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.InvoiceValue)).ToString() + "</strong>",
                        TDS = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.TDS)).ToString() + "</strong>",
                        TDSPlus = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.TDSPlus)).ToString() + "</strong>",
                        Amount = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.Amount)).ToString() + "</strong>",
                        ReceiptAmount = "<strong>" + objTDSMain.TDSReportLst.Select(m => new { CRNo = m.CRNo, ReceiptAmount = m.ReceiptAmount }).ToList().Distinct().ToList().Sum(m => Convert.ToDecimal(m.ReceiptAmount)).ToString() + "</strong>"

                    });

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objTDSMain.ObjTDSReporPartyWise.Add(
                        new TDSReporPartyWise
                        {
                            Party = Convert.ToString(Result["Party"]),
                            Tan = Convert.ToString(Result["Tan"]),
                            Value = Convert.ToString(Result["Value"]),
                            TDS = Convert.ToString(Result["TDS"]),
                            TDSPlus = Result["TDSPlus"].ToString()
                        });
                        //objTDSMain.ObjTDSReporPartyWise.Party = Convert.ToString(Result["Party"]);
                        //objTDSMain.ObjTDSReporPartyWise.Tan = Convert.ToString(Result["Tan"]);
                        //objTDSMain.ObjTDSReporPartyWise.Value = Convert.ToString(Result["Value"]);
                        //objTDSMain.ObjTDSReporPartyWise.TDS = Convert.ToString(Result["TDS"]);



                    }
                    if (Status == 1)
                    {
                        objTDSMain.ObjTDSReporPartyWise.Add(
                        new TDSReporPartyWise
                        {
                            Party = "<strong>Total</strong>",
                            Tan = string.Empty,
                            Value = "<strong>" + objTDSMain.ObjTDSReporPartyWise.ToList().Sum(m => Convert.ToDecimal(m.Value)).ToString() + "</strong>",
                            TDS = "<strong>" + objTDSMain.ObjTDSReporPartyWise.ToList().Sum(m => Convert.ToDecimal(m.TDS)).ToString() + "</strong>",
                            TDSPlus = "<strong>" + objTDSMain.ObjTDSReporPartyWise.ToList().Sum(m => Convert.ToDecimal(m.TDSPlus)).ToString() + "</strong>",
                        });
                    }
                }
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

        #endregion

        #region Pending TDS Report        

        public void PendingTdsReport(LNSM_TDSReport ObjTDSReport)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReportType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ObjTDSReport.ReportType });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PendingTDSReport", CommandType.StoredProcedure, DParam);
            LNSM_TDSMain objTDSMain = new LNSM_TDSMain();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    objTDSMain.TDSReportLst.Add(new LNSM_TDSReport
                    {
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),                        
                        TDS = Result["TDS"].ToString(),                        
                        Amount = Result["Amount"].ToString(),
                        ReceiptAmount = Result["ReceiptAmt"].ToString(),

                    });
                }
                if (Status == 1)
                {
                    objTDSMain.TDSReportLst.Add(new LNSM_TDSReport
                    {
                        InvoiceNo = "<strong>Total</strong>",
                        InvoiceDate = string.Empty,                        
                        TDS = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.TDS)).ToString() + "</strong>",                        
                        Amount = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.Amount)).ToString() + "</strong>",
                        ReceiptAmount = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.ReceiptAmount)).ToString() + "</strong>"

                    });

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objTDSMain.ObjTDSReporPartyWise.Add(
                        new TDSReporPartyWise
                        {
                            Party = Convert.ToString(Result["Party"]),
                            Tan = Convert.ToString(Result["Tan"]),                            
                            TDS = Convert.ToString(Result["TDS"]),                            
                            CertificateAdjusted = Result["CertificateAdjusted"].ToString(),
                            PendingTDS = Result["PendingTDS"].ToString()
                        });
                        //objTDSMain.ObjTDSReporPartyWise.Party = Convert.ToString(Result["Party"]);
                        //objTDSMain.ObjTDSReporPartyWise.Tan = Convert.ToString(Result["Tan"]);
                        //objTDSMain.ObjTDSReporPartyWise.Value = Convert.ToString(Result["Value"]);
                        //objTDSMain.ObjTDSReporPartyWise.TDS = Convert.ToString(Result["TDS"]);



                    }
                    if (Status == 1)
                    {
                        objTDSMain.ObjTDSReporPartyWise.Add(
                        new TDSReporPartyWise
                        {
                            Party = "<strong>Total</strong>",
                            Tan = string.Empty,                           
                            TDS = "<strong>" + objTDSMain.ObjTDSReporPartyWise.ToList().Sum(m => Convert.ToDecimal(m.TDS)).ToString() + "</strong>",                            
                            CertificateAdjusted = "<strong>" + objTDSMain.ObjTDSReporPartyWise.ToList().Sum(m => Convert.ToDecimal(m.CertificateAdjusted)).ToString() + "</strong>",
                            PendingTDS = "<strong>" + objTDSMain.ObjTDSReporPartyWise.ToList().Sum(m => Convert.ToDecimal(m.PendingTDS)).ToString() + "</strong>",
                        });
                    }
                }
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

        #endregion

        public void GetBulkCashreceiptForExternalUser(string FromDate, string ToDate, string ReceiptNo, int PartyId)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate == null ? "" : FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate == null ? "" : ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 30, Value = PartyId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBulkCashRecptForPrintForExternalUser", CommandType.StoredProcedure, DParam);
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

        #region Register of Outward Supply
        public void GetContainerWiseCharges(DateTime date1, DateTime date2)
        {
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
         
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            DataSet ds = DataAccess.ExecuteDataSet("GetContainerWiseOutward", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
           
                List<LNSM_ContainerWiseCharges> model = new List<LNSM_ContainerWiseCharges>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        model = (from DataRow dr in dt.Rows
                                 select new LNSM_ContainerWiseCharges()
                                 {
                                     SlNo = Convert.ToInt32(dr["SlNo"]),
                                     InvoiceNo = dr["InvoiceNo"].ToString(),
                                     InvoiceDate = dr["InvoiceDate"].ToString(),
                                     ContNo = dr["ContNo"].ToString(),
                                     Taxable = Convert.ToDecimal(dr["Taxable"]),
                                     IGSTAmt = Convert.ToDecimal(dr["IGSTAmt"]),
                                     CGSTAmt = Convert.ToDecimal(dr["CGSTAmt"]),
                                     SGSTAmt = Convert.ToDecimal(dr["SGSTAmt"]),
                                     Total = Convert.ToDecimal(dr["Total"]),

                                 }).ToList();
                    }
                    decimal InvoiceAmount = 0, CRAmount = 0;

                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = ContainerWiseChargesExcel(model, InvoiceAmount, CRAmount);
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
        private string ContainerWiseChargesExcel(List<LNSM_ContainerWiseCharges> model, decimal InvoiceAmount, decimal CRAmount)
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
                        + "";
                exl.MargeCell("A1:I1", title, DynamicExcel.CellAlignment.TopCenter);             
                exl.MargeCell("A2:A4", "Sl.No.", DynamicExcel.CellAlignment.CenterLeft);
                exl.MargeCell("B2:B4", "Invoice No", DynamicExcel.CellAlignment.CenterLeft);
                exl.MargeCell("C2:C4", "Invoice Date", DynamicExcel.CellAlignment.CenterLeft);
                exl.MargeCell("D2:D4", "Container No", DynamicExcel.CellAlignment.CenterLeft);
                exl.MargeCell("E2:E4", "Taxable", DynamicExcel.CellAlignment.CenterLeft);
                exl.MargeCell("F2:F4", "IGST Amt", DynamicExcel.CellAlignment.CenterLeft);
                exl.MargeCell("G2:G4", "CGST Amt", DynamicExcel.CellAlignment.CenterLeft);
                exl.MargeCell("H2:H4", "SGST Amt" , DynamicExcel.CellAlignment.CenterLeft);
                exl.MargeCell("I2:I4", "Total", DynamicExcel.CellAlignment.CenterLeft);


              

                exl.AddTable<LNSM_ContainerWiseCharges>("A",5, model, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 20});
                var serviceValue = model.Sum(o => o.Taxable);
                var igstamt = model.Sum(o => o.IGSTAmt);
                var sgstamt = model.Sum(o => o.SGSTAmt);
                var cgstamt = model.Sum(o => o.CGSTAmt);
                var totalamt = model.Sum(o => o.Total);
                exl.AddCell("A" + (model.Count + 5).ToString(), "Total", DynamicExcel.CellAlignment.CenterLeft);
                exl.AddCell("E" + (model.Count + 5).ToString(), serviceValue.ToString(), DynamicExcel.CellAlignment.CenterLeft);
                exl.AddCell("F" + (model.Count + 5).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.CenterLeft);
                exl.AddCell("G" + (model.Count + 5).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.CenterLeft);
                exl.AddCell("H" + (model.Count + 5).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.CenterLeft);
                exl.AddCell("I" + (model.Count + 5).ToString(), totalamt.ToString(), DynamicExcel.CellAlignment.CenterLeft);


                exl.Save();
            }
            return excelFile;
        }
       
        #endregion

    }
}
