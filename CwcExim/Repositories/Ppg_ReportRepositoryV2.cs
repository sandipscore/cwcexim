using CwcExim.Areas.Auction.Models;
using CwcExim.Areas.Report.Models;
using CwcExim.DAL;
using CwcExim.Models;
using EinvoiceLibrary;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CwcExim.Repositories
{
    public class Ppg_ReportRepositoryV2
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {              
            get
            {
                return _DBResponse;
            }
        }

        #region Bulk invoice 
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
            IDataReader Result = DataAccess.ExecuteDataReader("InvoiceListWithDateV2", CommandType.StoredProcedure, DParam);
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
                Result = DataAccess.ExecuteDataSet("GetBulkInvoiceDetailsForPrintV2", CommandType.StoredProcedure, DParam);
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
            LstInvoice = DataAccess.ExecuteDataSet("ModuleListWithInvoiceV2", CommandType.StoredProcedure, DParam);

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
        public void GenericBulkInvoiceDetailsForPrintAuction(AuctionInvoiceViewModel ObjBulkInvoiceReport)
        {
            DateTime dtfrom = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            ObjBulkInvoiceReport.InvoiceModule = "Auc";
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
                Result = DataAccess.ExecuteDataSet("GetAuctioninvoicedetailsforprint", CommandType.StoredProcedure, DParam);
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


            List<PPGRegisterOfEInvoiceModel> model = new List<PPGRegisterOfEInvoiceModel>();
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


        private string RegisterofEInvoiceExcel(List<PPGRegisterOfEInvoiceModel> model, DataTable dt)
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
            PPG_BulkIRN objInvoice = new PPG_BulkIRN();
            IDataReader Result = DataAccess.ExecuteDataReader("IRNNotgeneratedInvoiceList", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    objInvoice.lstPostPaymentChrg.Add(new PPG_BulkIRNDetails
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
            List<Ppg_E04Report> LstE04 = new List<Ppg_E04Report>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstE04.Add(new Ppg_E04Report
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
            Ppg_E04Report objE04Report = new Ppg_E04Report();
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
            List<Ppg_E04Report> LstE04Report = new List<Ppg_E04Report>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstE04Report.Add(new Ppg_E04Report
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
            List<Ppg_ContStufAckSearch> LstStuffing = new List<Ppg_ContStufAckSearch>();
            try
            {
                bool State = false;     
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Ppg_ContStufAckSearch
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
            List<Ppg_ContStufAckSearch> LstStuff = new List<Ppg_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Ppg_ContStufAckSearch
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
        public void GetStufAckResult(string container, string shipbill, string cfscode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_container", MySqlDbType = MySqlDbType.VarChar, Value = container });
            LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Value = shipbill });
            LstParam.Add(new MySqlParameter { ParameterName = "in_cfscode", MySqlDbType = MySqlDbType.VarChar, Value = cfscode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStufAckResult", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_ContStufAckRes> Lststufack = new List<Ppg_ContStufAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Ppg_ContStufAckRes
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

        #region SBQueryReport
        public void GetAllSB()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShipBillListForSBQuery", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_SBQuery> LstSB = new List<PPG_SBQuery>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new PPG_SBQuery
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        SBNODate = Result["SBNODate"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
                }
                else
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

        public void SBQueryReport(int id, string sbno, string sbdate)
        {


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBNO", MySqlDbType = MySqlDbType.VarChar, Value = sbno });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBDate", MySqlDbType = MySqlDbType.Date, Value = sbdate });

            int Status = 0;

            IDataParameter[] DParam = { };



            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSBQueryDetails", CommandType.StoredProcedure, DParam);
            PPG_SBQuery LstSBQueryReport = new PPG_SBQuery();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSBQueryReport.Id = Convert.ToInt32(Result["Id"]);
                    LstSBQueryReport.SBNO = Convert.ToString(Result["SBNO"]);
                    LstSBQueryReport.PortOFDischarge = Convert.ToString(Result["PortOfDischarge"]);
                    LstSBQueryReport.PortOFLoad = Convert.ToString(Result["PortName"]);
                    LstSBQueryReport.ShippingLine = Convert.ToString(Result["ShippingLine"]);
                    LstSBQueryReport.Comodity = Convert.ToString(Result["Comodity"]);
                    LstSBQueryReport.CHA = Convert.ToString(Result["CHA"]);
                    LstSBQueryReport.Date = Convert.ToString(Result["Date"]);
                    //LstSBQueryReport.PortOFDischarge = Convert.ToString(Result["Id"]);
                    LstSBQueryReport.Package = Convert.ToInt32(Result["Package"]);
                    LstSBQueryReport.Weight = Convert.ToDecimal(Result["Weight"]);
                    LstSBQueryReport.FOB = Convert.ToDecimal(Result["FOB"]);
                    LstSBQueryReport.Cargotype = Convert.ToInt32(Result["CargoType"]);
                    LstSBQueryReport.Vehicle = Convert.ToString(Result["NoOfVehicle"]);
                    LstSBQueryReport.Exporter = Convert.ToString(Result["Exporter"]);
                    LstSBQueryReport.Country = Convert.ToString(Result["Country"]);
                    LstSBQueryReport.GateinNo = Convert.ToString(Result["GateInNo"]);
                    LstSBQueryReport.PCIN = Convert.ToString(Result["PCIN"]);
                    //LstSBQueryReport.InvoiceNo = Convert.ToString(Result["InvoiceNo"]);

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSBQueryReport;
                }
                else
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
        #region PortwiseTeus
        public void PortTeus(PPG_teus_search ObjTEUS)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjTEUS.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjTEUS.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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

            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = ObjTEUS.Type });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("portwiseteus", CommandType.StoredProcedure, DParam);
            IList<PPG_teus_search> Lsteus = new List<PPG_teus_search>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    Lsteus.Add(new PPG_teus_search
                    {

                        PortName = Result["PortName"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        TrainNo = Result["TrainNo"].ToString(),
                        TrainDate = Result["TrainDate"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
                      

                    });
                }
            

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lsteus;
                }
                else
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
        #region Stuffing ASR Acknowledgement Search       

        public void GetCotainerNoForASRAck(string in_cont, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_cont", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = in_cont });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCotainerNoForASRAckStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_ContStufAckSearch> LstStuff = new List<Ppg_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Ppg_ContStufAckSearch
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
        public void GetAllShippingBillNoForASRACK(string shipbill, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingBillNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = shipbill });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetshippingbillNoForASRAckStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_ContStufAckSearch> LstStuff = new List<Ppg_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Ppg_ContStufAckSearch
                    {
                        shippingbillno = Result["ShippingBillNo"].ToString(),
                        shippingbilldate = Result["ShippingBillDate"].ToString(),
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
        public void GetASRAckResult(string shipbill, string CFSCode, string container)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_container", MySqlDbType = MySqlDbType.VarChar, Value = container });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Value = shipbill });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetASRAckResult", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_ContStufAckRes> Lststufack = new List<Ppg_ContStufAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Ppg_ContStufAckRes
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

        #region DP Acknowledment Serach  

        public void GetGatePassNoDPForAckSearch(string GatePassNo, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = GatePassNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGatePassNoDPForAckSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_GatePassDPAckSearch> lstDPGPAck = new List<Ppg_GatePassDPAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDPGPAck.Add(new Ppg_GatePassDPAckSearch
                    {
                        GatePassId = Convert.ToInt32(Result["GatePassId"]),
                        GatePassNo = Convert.ToString(Result["GatePassNo"])
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
                    _DBResponse.Data = new { lstDPGPAck, State };
                }
                else
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
        public void GetContainerNoForDPAckSearch(string ContainerNo, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNoForDPAckSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            List<Ppg_ContDPAckSearch> lstDPContACK = new List<Ppg_ContDPAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDPContACK.Add(new Ppg_ContDPAckSearch
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        GatePassdtlId = Convert.ToInt32(Result["GatePassdtlId"]),
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
                    _DBResponse.Data = new { lstDPContACK, State };
                }
                else
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
        public void GetDPAckSearch(int GatePassId, string ContainerNo, int GatePassDetailId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.VarChar, Value = GatePassId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassDetailId", MySqlDbType = MySqlDbType.VarChar, Value = GatePassDetailId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDPAckResult", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_DPAckRes> Lststufack = new List<Ppg_DPAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Ppg_DPAckRes
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Path = Result["Path"].ToString(),
                        Reason = Result["Reason"].ToString(),
                        Status = Result["Status"].ToString(),
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

        #region DT Acknowledment Serach  

        public void GetGatePassNoDTForAckSearch(string GatePassNo, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = GatePassNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGatePassNoDTForAckSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_GatePassDTAckSearch> lstDTGPAck = new List<Ppg_GatePassDTAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDTGPAck.Add(new Ppg_GatePassDTAckSearch
                    {
                        GatePassId = Convert.ToInt32(Result["GatePassId"]),
                        GatePassNo = Convert.ToString(Result["GatePassNo"])
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
                    _DBResponse.Data = new { lstDTGPAck, State };
                }
                else
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
        public void GetContainerNoForDTAckSearch(string ContainerNo, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNoForDTAckSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            List<Ppg_ContDTAckSearch> lstDTContACK = new List<Ppg_ContDTAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDTContACK.Add(new Ppg_ContDTAckSearch
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
                    _DBResponse.Data = new { lstDTContACK, State };
                }
                else
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
        public void GetDTAckSearch(int GatePassId, string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = GatePassId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDTAckResult", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_DTAckRes> Lststufack = new List<Ppg_DTAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Ppg_DTAckRes
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Reason = Result["Reason"].ToString(),
                        Status = Result["Status"].ToString(),
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

        #region Stuffing Loaded Search       


        public void GetStufloadResult(string jobno)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_jobno", MySqlDbType = MySqlDbType.VarChar, Value = jobno });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Value = shipbill });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_cfscode", MySqlDbType = MySqlDbType.VarChar, Value = cfscode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getstufloadno", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Kol_loadstuf> Lststufack = new List<Kol_loadstuf>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Kol_loadstuf
                    {
                        loadstufreqno = Result["loadreqno"].ToString(),
                        expstufreqno = Result["stufreqno"].ToString(),
                        // reason = Result["reason"].ToString(),
                        // status = Result["status"].ToString(),
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
        public void GetStufloadasrResult(string jobasrno)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_asrjobno", MySqlDbType = MySqlDbType.VarChar, Value = jobasrno });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Value = shipbill });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_cfscode", MySqlDbType = MySqlDbType.VarChar, Value = cfscode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getstufloadasrno", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Kol_loadstufasr> Lststufack = new List<Kol_loadstufasr>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Kol_loadstufasr
                    {
                        loadstufasrreqno = Result["loadasrreqno"].ToString(),
                        expstufasrreqno = Result["stufasrreqno"].ToString(),
                        // reason = Result["reason"].ToString(),
                        // status = Result["status"].ToString(),
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
        public void GetStufloaddpResult(string jobdpno)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_dpjobno", MySqlDbType = MySqlDbType.VarChar, Value = jobdpno });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Value = shipbill });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_cfscode", MySqlDbType = MySqlDbType.VarChar, Value = cfscode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getstufloaddpno", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Kol_loadstufdp> Lststufack = new List<Kol_loadstufdp>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Kol_loadstufdp
                    {
                        loadstufdpreqno = Result["loaddpreqno"].ToString(),
                        expstufdpreqno = Result["stufdpreqno"].ToString(),
                        // reason = Result["reason"].ToString(),
                        // status = Result["status"].ToString(),
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
        public void GetStufloaddtResult(string jobdtno)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_dtjobno", MySqlDbType = MySqlDbType.VarChar, Value = jobdtno });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Value = shipbill });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_cfscode", MySqlDbType = MySqlDbType.VarChar, Value = cfscode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getstufloaddtno", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Kol_loadstufdt> Lststufack = new List<Kol_loadstufdt>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Kol_loadstufdt
                    {
                        loadstufdtreqno = Result["loaddtreqno"].ToString(),
                        expstufdtreqno = Result["stufdtreqno"].ToString(),
                        // reason = Result["reason"].ToString(),
                        // status = Result["status"].ToString(),
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

        #region Tally response

        public void GetTallyResponse(TallyResponse vm)
        {

            DateTime dtfrom = DateTime.ParseExact(vm.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(vm.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("TallyWebApiResponse", CommandType.StoredProcedure, DParam);
            IList<TallyResponse> lstData = new List<TallyResponse>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstData.Add(new TallyResponse
                    {
                        Date = Result["Date"].ToString(),
                        Bill = Result["Bill"].ToString(),
                        Invoice = Result["Invoice"].ToString(),
                        Dr = Result["Dr"].ToString(),
                        Cr = Result["Cr"].ToString(),
                        Receipt = Result["Receipt"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstData;
                }
                else
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

        #region Bulk Invoice For external User

        public void GenericBulkInvoiceDetailsForPrintForExternalUser(BulkInvoiceReport ObjBulkInvoiceReport)
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
                Result = DataAccess.ExecuteDataSet("GetBulkInvoiceDetailsForPrintV2ForExternalUser", CommandType.StoredProcedure, DParam);
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
        public void GetInvoiceListForExternalUser(string FromDate, string ToDate, string invoiceType,int Uid)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("InvoiceListWithDateV2ForExternalUser", CommandType.StoredProcedure, DParam);
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
        #endregion


        #region Bulk QR Code Generation

        public void GetBulkQRDetails(string FromDate,string ToDate )
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            PPG_BulkIRN objInvoice = new PPG_BulkIRN();
           
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.String, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.String, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIRNNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    objInvoice.lstPostPaymentChrg.Add(new PPG_BulkIRNDetails
                    {
                        InvoiceId = Convert.ToInt32(Result["id"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),                      
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                         GstNo= Result["irn"].ToString(),
                          InvoiceType= Result["TYPE"].ToString(),
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

        public void AddEditQRResponsec(IrnResponse objOBL, string IRNNo)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
          
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
            int Result = DA.ExecuteNonQuery("AddeditQRresponce", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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


        // Susmita Patra - 27/05/2023

        #region Gate Pass Section
        public void GetAreaDetailsGPReport(DateTime date1, DateTime date2)
        {
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetAreaDetailsGP", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];


            List<AreaDetailsGPModel> model = new List<AreaDetailsGPModel>();
            try
            {

                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = GetAreaDetailsGPExcel(model, dt);
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



        private string GetAreaDetailsGPExcel(List<AreaDetailsGPModel> model, DataTable dt)
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


        #region Open Calculator

        public void OpenCalculator(OpenCalculator vm)
        {
            string EntryDate = null;
            string DeliveryDate = null;

            string CartingRegisterDare = null;
            string CCINDate = null;

            if (!string.IsNullOrEmpty(vm.EntryDate))
            {
                DateTime dtEntryDate = DateTime.ParseExact(vm.EntryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                 EntryDate = dtEntryDate.ToString("yyyy/MM/dd");
            }
            if (!string.IsNullOrEmpty(vm.DeliveryDate))
            {
                DateTime dtDeliveryDate = DateTime.ParseExact(vm.DeliveryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DeliveryDate = dtDeliveryDate.ToString("yyyy/MM/dd");
            }
            if (!string.IsNullOrEmpty(vm.Carting))
            {
                DateTime dtCarting = DateTime.ParseExact(vm.Carting, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                CartingRegisterDare = dtCarting.ToString("yyyy/MM/dd");
            }
            if (!string.IsNullOrEmpty(vm.CCIN))
            {
                DateTime dtCCIN = DateTime.ParseExact(vm.CCIN, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                CCINDate = dtCCIN.ToString("yyyy/MM/dd");
            }







            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = DeliveryDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryDate", MySqlDbType = MySqlDbType.DateTime, Value = EntryDate });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterDare", MySqlDbType = MySqlDbType.DateTime, Value = CartingRegisterDare });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CCINDate", MySqlDbType = MySqlDbType.DateTime, Value = CCINDate });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = vm.CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Value =vm.OperationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementType", MySqlDbType = MySqlDbType.VarChar, Value = vm.MovementType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.Int32, Value = vm.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CBM", MySqlDbType = MySqlDbType.Decimal, Value =Convert.ToDecimal(string.IsNullOrEmpty(vm.CBM)?"0": vm.CBM) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CIFValue", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(string.IsNullOrEmpty(vm.CIFValue) ? "0" : vm.CIFValue) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Duty", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(string.IsNullOrEmpty(vm.Duty) ? "0" : vm.Duty) });


            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CalculatorOpenCharge", CommandType.StoredProcedure, DParam);
            IList<ChargesList> lstData = new List<ChargesList>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstData.Add(new ChargesList
                    {
                        ChargeName = Result["ChargeName"].ToString(),
                        Amount = Convert.ToDecimal(Result["Taxable"])
                       

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstData;
                }
                else
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

        #region Charge Calculator

        public void ChargeCalculator(ChargeCalculator vm)
        {
           
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "P_OBL_NO", MySqlDbType = MySqlDbType.VarChar, Value = vm.OBLNo });
            LstParam.Add(new MySqlParameter { ParameterName = "P_CIF_VALUE", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.CIFValue) });
            LstParam.Add(new MySqlParameter { ParameterName = "P_DUTY_VALUE", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.Duty) });
            LstParam.Add(new MySqlParameter { ParameterName = "P_USER_CODE", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PORTALCHARGEVIEW", CommandType.StoredProcedure, DParam);
            IList<ChargesList> lstData = new List<ChargesList>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstData.Add(new ChargesList
                    {
                        ChargeName = Result["CHARGE_NAME"].ToString(),
                        Amount = Convert.ToDecimal(Result["VALUE"])


                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstData;
                }
                else
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

        #region SD Ledger

        public void GetBillCumSDLedgerReport(string dtFromdate, string dtTodate, int strParty)
        {
            dtFromdate = DateTime.ParseExact(dtFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime dtFrom = DateTime.ParseExact(dtFromdate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dtTodate = DateTime.ParseExact(dtTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime dtTo = DateTime.ParseExact(dtTodate, "MM/dd/yyyy", CultureInfo.InvariantCulture);


            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = dtFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = dtTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = strParty });
       

            DParam = LstParam.ToArray();
            DataSet dspdareport = DataAccess.ExecuteDataSet("CashReceiptInvoiceLedger", CommandType.StoredProcedure, DParam);
            CashReceiptInvoiceLedger CrInvLedgerObj = new CashReceiptInvoiceLedger();
            _DBResponse = new DatabaseResponse();
            try
            {
                if (dspdareport.Tables[0].Rows.Count > 0 && dspdareport.Tables[3].Rows.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow Result in dspdareport.Tables[0].Rows)
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

                    CrInvLedgerObj.lstLedgerSummary = new List<CrInvLedgerSummary>();
                    foreach (DataRow Result in dspdareport.Tables[1].Rows)
                    {
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

                    CrInvLedgerObj.lstLedgerDetails = new List<CrInvLedgerDetails>();
                    foreach (DataRow Result in dspdareport.Tables[2].Rows)
                    {
                        CrInvLedgerObj.lstLedgerDetails.Add(new CrInvLedgerDetails
                        {
                            InvCr = Convert.ToInt32(Result["InvCr"]),
                            InvCrId = Convert.ToInt32(Result["InvCrId"]),
                            Description = Convert.ToString(Result["Description"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                        });
                    }

                    CrInvLedgerObj.lstLedgerDetailsFull = new List<CrInvLedgerFullDetails>();
                    foreach (DataRow Result in dspdareport.Tables[3].Rows)
                    {
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
                            GroupSr = Convert.ToString(Result["GroupSr"]),
                        });
                    }



                    //CrInvLedgerObj.CompanyName = comname;
                    //CrInvLedgerObj.CompanyAddress = address;

                    CrInvLedgerObj.lstLedgerSummary.ForEach(item =>
                    {
                        item.LedgerDetails = new List<CrInvLedgerDetails>();
                        var dtls = CrInvLedgerObj.lstLedgerDetails.Where(o => o.InvCr == item.InvCr && o.InvCrId == item.InvCrId).ToList();
                        dtls.ForEach(d =>
                        {
                            item.LedgerDetails.Add(d);
                        });
                    });

                    CrInvLedgerObj.TotalDebit = CrInvLedgerObj.lstLedgerDetailsFull.Sum(x => x.Debit);
                    CrInvLedgerObj.TotalCredit = CrInvLedgerObj.lstLedgerDetailsFull.Sum(x => x.Credit);
                    CrInvLedgerObj.ClosingBalance = CrInvLedgerObj.OpenningBalance + CrInvLedgerObj.TotalCredit - CrInvLedgerObj.TotalDebit;


                }

                if (Status == 1)
                {
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
               // Result.Close();
              //  Result.Dispose();

            }
        }
        #endregion

        #region Tds Deduction Report       

        public void TdsDeductionExcelRpt(string PeriodFrom, string PeriodTo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodFrom).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodTo).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetTDSDetailsRO", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Status = 1;
            try
            {

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
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
                //  Result.Close();
            }
        }
        #endregion

        #region --OOC Details--

        public void GetOocList()
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOocList", CommandType.StoredProcedure, DParam);
            IList<PpgOocDetailReport> LstOoc = new List<PpgOocDetailReport>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstOoc.Add(new PpgOocDetailReport
                    {
                        ID = Convert.ToInt32(Result["ID"]), 
                        BOE_NO = Result["BOE_NO"].ToString(),
                        BOE_DATE = Result["BOE_DATE"].ToString(),
                        IGM_NO = Result["IGM_NO"].ToString(),
                        IGM_DATE = Result["IGM_DATE"].ToString(),
                        LINE_NO = Result["LINE_NO"].ToString(),
                        SUB_LINE_NO = Result["SUB_LINE_NO"].ToString(),
                        OOC_TYPE = Result["OOC_TYPE"].ToString(),
                        OOC_NO = Result["OOC_NO"].ToString(),
                        OOC_DATE = Result["OOC_DATE"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstOoc;
                }
                else
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

        public void GetOocListByBoeNo(string BOE_NO, string BOE_DATE)
        {

            DateTime dtfrom = DateTime.ParseExact(BOE_DATE, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOE_NO", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = BOE_NO });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOE_DATE", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOocListByBoeNo", CommandType.StoredProcedure, DParam);
            IList<PpgOocDetailReport> LstOoc = new List<PpgOocDetailReport>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstOoc.Add(new PpgOocDetailReport
                    {
                        ID = Convert.ToInt32(Result["ID"]),
                        BOE_NO = Result["BOE_NO"].ToString(),
                        BOE_DATE = Result["BOE_DATE"].ToString(),
                        IGM_NO = Result["IGM_NO"].ToString(),
                        IGM_DATE = Result["IGM_DATE"].ToString(),
                        LINE_NO = Result["LINE_NO"].ToString(),
                        SUB_LINE_NO = Result["SUB_LINE_NO"].ToString(),
                        OOC_TYPE = Result["OOC_TYPE"].ToString(),
                        OOC_NO = Result["OOC_NO"].ToString(),
                        OOC_DATE = Result["OOC_DATE"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstOoc;
                }
                else
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

        public void GetOocDetailById(int Id)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOocDetailById", CommandType.StoredProcedure, DParam);
            PpgOocDetailReport LstOoc = new PpgOocDetailReport();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;                    
                        LstOoc.ID = Convert.ToInt32(Result["ID"]);
                        LstOoc.BOE_NO = Result["BOE_NO"].ToString();
                        LstOoc.BOE_DATE = Result["BOE_DATE"].ToString();
                        LstOoc.IGM_NO = Result["IGM_NO"].ToString();
                        LstOoc.IGM_DATE = Result["IGM_DATE"].ToString();
                        LstOoc.LINE_NO = Result["LINE_NO"].ToString();
                        LstOoc.SUB_LINE_NO = Result["SUB_LINE_NO"].ToString();
                        LstOoc.OOC_TYPE = Result["OOC_TYPE"].ToString();
                        LstOoc.OOC_NO = Result["OOC_NO"].ToString();
                        LstOoc.OOC_DATE = Result["OOC_DATE"].ToString();
                        LstOoc.MSG_TYPE = Result["MSG_TYPE"].ToString();
                        LstOoc.CUSTOM_CD = Result["CUSTOM_CD"].ToString();
                        LstOoc.NO_PKG = Result["NO_PKG"].ToString();
                        LstOoc.PKG_TYPE = Result["PKG_TYPE"].ToString();
                        LstOoc.GR_WT = Result["GR_WT"].ToString();
                        LstOoc.UNIT_TYPE = Result["UNIT_TYPE"].ToString();
                        LstOoc.CIF_VALUE = Result["CIF_VALUE"].ToString();
                        LstOoc.DUTY = Result["DUTY"].ToString();
                        LstOoc.ASM_NO = Result["ASM_NO"].ToString();
                        LstOoc.G_DATE = Result["G_DATE"].ToString();                    
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstOoc;
                }
                else
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

        #region Party Ledger Statement for External User
        public void GetPartyLedgerReportExternalUser(int PartyId, string Fdt, string Tdt)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.VarString, Value = Fdt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.VarString, Value = Tdt });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("RptPartyLedgerStatementEU", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Ppg_PartyLedger OAResult = new Ppg_PartyLedger();
            try
            {

                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        OAResult.PartyName = Convert.ToString(dr["PartyName"]);
                        OAResult.PartyCode = Convert.ToString(dr["PartyCode"]);
                        OAResult.PartyGst = Convert.ToString(dr["PartyGst"]);                       
                    }

                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        OAResult.LstOnAccountDtl.Add(new Ppg_LedgerStatementList
                        {
                            ReceiptNo = Convert.ToString(dr["ReceiptNo"]),
                            ReceivedDate = Convert.ToString(dr["ReceiptDate"]),
                            ChargeCode = Convert.ToString(dr["ChargeCode"]),
                            DepositAmt = Convert.ToDecimal(dr["CreditAmt"]),
                            Cheque_No = Convert.ToString(dr["Cheque_No"]),
                            InvAmt = Convert.ToDecimal(dr["DebitAmt"]),                           
                            ContainerNo = Convert.ToString(dr["Container_Shipping"])
                        });
                    }

                    foreach (DataRow dr in Result.Tables[2].Rows)
                    {
                        OAResult.Deposit = dr["Deposit"] == System.DBNull.Value ? 0 : Convert.ToDecimal(dr["Deposit"]);
                        OAResult.Invoice = dr["Invoice"] == System.DBNull.Value ? 0 : Convert.ToDecimal(dr["Invoice"]);
                        OAResult.Balance = dr["Balance"] == System.DBNull.Value ? 0 : Convert.ToDecimal(dr["Balance"]);                       
                    }

                    foreach (DataRow dr in Result.Tables[3].Rows)
                    {
                        OAResult.Opening = dr["Opening"] == System.DBNull.Value ? 0 : Convert.ToDecimal(dr["Opening"]);                       
                    }

                    foreach (DataRow dr in Result.Tables[4].Rows)
                    {
                        OAResult.SDBalance = dr["SDBalance"] == System.DBNull.Value ? 0 : Convert.ToDecimal(dr["SDBalance"]);                       
                    }

                    foreach (DataRow dr in Result.Tables[5].Rows)
                    {
                        OAResult.LstOnSum.Add(new Ppg_Summary
                        {
                            ReceiptNo = Convert.ToString(dr["ReceiptNo"]),
                            Total = Convert.ToDecimal(dr["Total"])                           
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OAResult;
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

        #region PdConsolidateReport  for external user

        public void PdConsolidateReportForExternal(Ppg_SDBalaceForEU ObjPLC, int PartyId)
        {

            DateTime dtto = DateTime.ParseExact(ObjPLC.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtto.ToString("yyyy/MM/dd");
            
            _DBResponse = new DatabaseResponse();

            int Status = 0;
            
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            
            DParam = LstParam.ToArray();
            DataSet ds = DataAccess.ExecuteDataSet("RptPartyLedgerConsolidateForExternalUser", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];

            List<Ppg_SDBalaceForEU> model = new List<Ppg_SDBalaceForEU>();
            try
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = PLedgerConsolidateExcel(model, dt, ObjPLC.PeriodTo);
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

        private string PLedgerConsolidateExcel(List<Ppg_SDBalaceForEU> model, DataTable dt, string datevalue)
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
              

                System.Web.UI.WebControls.TableCell cell1 = new System.Web.UI.WebControls.TableCell();
                cell1.Text = "<b> CENTRAL WAREHOUSING CORPORATION </b> ";
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
                cell3.Text = "PATPARGANJ";
                cell3.ForeColor = System.Drawing.Color.Black;

                System.Web.UI.WebControls.TableRow tr3 = new System.Web.UI.WebControls.TableRow();
                tr3.Cells.Add(cell3);
                tr3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;


                System.Web.UI.WebControls.TableCell cell4 = new System.Web.UI.WebControls.TableCell();
                cell4.Text = "Consolidate Party Ledger Statement As On Date " + datevalue + "";
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

        #region KnowYourSDBalance
        public void KnowYourSDBalaceForExternal(string ToDate, int PartyId)
        {

            DateTime dtfrom = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");            
            _DBResponse = new DatabaseResponse();

            int Status = 0;
           
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fdt", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
                     
            DParam = LstParam.ToArray();
            DataSet ds = DataAccess.ExecuteDataSet("RptSDBalaceForExternal", CommandType.StoredProcedure, DParam);

            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }           
            
            List<Ppg_SDBalaceForEU> model = new List<Ppg_SDBalaceForEU>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        model.Add(new Ppg_SDBalaceForEU
                        {
                            BalAmount = Convert.ToString(dr["UtilizationAmount"]),
                            SDAmount = Convert.ToString(dr["SDAmount"]),
                            PartyName = Convert.ToString(dr["PartyName"]),
                        });
                    }

                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = model;
                }
                else
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
                ds.Dispose();
            }




        }
        #endregion

        //#region BOE No  QUERY External User
        //public void ListOfICEGateBOENoForExternalUser(string SearchBy, string OBLNo, int PartyId, int Page = 0)
        //{
        //    int Status = 0;
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    List<MySqlParameter> lstParam = new List<MySqlParameter>();
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = OBLNo });
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
        //    lstParam.Add(new MySqlParameter { ParameterName = "In_SearchBy", MySqlDbType = MySqlDbType.VarChar, Value = SearchBy });
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
        //    IDataParameter[] Dparam = lstParam.ToArray();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetBOENoForOBLSearchExternalUser", CommandType.StoredProcedure, Dparam);
        //    IList<PpgOBLNoForPage> LstObl = new List<PpgOBLNoForPage>();
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        bool State = false;
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            LstObl.Add(new PpgOBLNoForPage
        //            {
        //                OBLNo = Result["OBLNo"].ToString()
        //            });
        //        }
        //        //if (Result.NextResult())
        //        //{
        //        //    while (Result.Read())
        //        //    {
        //        //        State = Convert.ToBoolean(Result["State"]);
        //        //    }
        //        //}
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = new { LstObl, State };
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

        //public void ListOfICEGateBOENo(string SearchBy, string OBLNo, int Page = 0)
        //{
        //    int Status = 0;
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    List<MySqlParameter> lstParam = new List<MySqlParameter>();
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = OBLNo });
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
        //    lstParam.Add(new MySqlParameter { ParameterName = "In_SearchBy", MySqlDbType = MySqlDbType.VarChar, Value = SearchBy });
        //    IDataParameter[] Dparam = lstParam.ToArray();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetBOENoForOBLSearch", CommandType.StoredProcedure, Dparam);
        //    IList<PpgOBLNoForPage> LstObl = new List<PpgOBLNoForPage>(); 
        //     _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        bool State = false;
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            LstObl.Add(new PpgOBLNoForPage
        //            {
        //                OBLNo = Result["OBLNo"].ToString()
        //            });
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                State = Convert.ToBoolean(Result["State"]);
        //            }
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = new { LstObl, State };
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
        //public void GetICEGateBOEDetail(string OBLNo, string SearchBy)
        //{
        //    //DateTime dtobl = DateTime.ParseExact(obldate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    //string obldt = dtobl.ToString("yyyy/MM/dd");
        //    DataSet Result = new DataSet();
        //    int Status = 0;
        //    try
        //    {
        //        //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //        IDataParameter[] DParam = { };
        //        List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //        DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //        LstParam.Add(new MySqlParameter { ParameterName = "In_OBLNo", MySqlDbType = MySqlDbType.String, Value = OBLNo });
        //        // LstParam.Add(new MySqlParameter { ParameterName = "In_OBLDate", MySqlDbType = MySqlDbType.DateTime, Value = obldt });
        //        LstParam.Add(new MySqlParameter { ParameterName = "In_SearchBy", MySqlDbType = MySqlDbType.String, Value = SearchBy });
        //        DParam = LstParam.ToArray();
        //        Result = DataAccess.ExecuteDataSet("BOEQuery", CommandType.StoredProcedure, DParam);

        //        _DBResponse = new DatabaseResponse();

        //        Ppg_BOEQuery objOBLEntry = new Ppg_BOEQuery();

        //        //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();

        //        if (Result != null && Result.Tables.Count > 0)
        //        {
        //            Status = 1;
        //            if (Result.Tables[0].Rows.Count > 0)
        //            {
        //                objOBLEntry.OBL_No = Convert.ToString(Result.Tables[0].Rows[0]["OBL_NO"]);
        //                objOBLEntry.OBL_Date = Convert.ToString(Result.Tables[0].Rows[0]["OBL_DATE"]);
        //                objOBLEntry.IGM_No = Convert.ToString(Result.Tables[0].Rows[0]["IGM_NO"]);
        //                objOBLEntry.IGM_Date = Convert.ToString(Result.Tables[0].Rows[0]["IGM_DATE"]);
        //                objOBLEntry.TPNo = Convert.ToString(Result.Tables[0].Rows[0]["TP_NO"]);
        //                objOBLEntry.TPDate = Convert.ToString(Result.Tables[0].Rows[0]["TP_DATE"]);
        //                objOBLEntry.MovementType = Convert.ToString(Result.Tables[0].Rows[0]["MovementType"]);
        //                objOBLEntry.PortId = Convert.ToInt32(Result.Tables[0].Rows[0]["PortId"]);
        //                objOBLEntry.LineNo = Convert.ToString(Result.Tables[0].Rows[0]["LINE_NO"].ToString());
        //                objOBLEntry.CargoDescription = Convert.ToString(Result.Tables[0].Rows[0]["CargoDescription"]);
        //                objOBLEntry.NoOfPkg = Convert.ToString(Result.Tables[0].Rows[0]["NO_PKG"]);
        //                objOBLEntry.PkgType = Convert.ToString(Result.Tables[0].Rows[0]["PKG_TYPE"]);
        //                objOBLEntry.GR_WT = Convert.ToDecimal(Result.Tables[0].Rows[0]["GR_WT"]);
        //                objOBLEntry.ImporterId = Convert.ToInt32(Result.Tables[0].Rows[0]["ImporterId"]);
        //                objOBLEntry.ImporterName = Convert.ToString(Result.Tables[0].Rows[0]["ImporterName"]);
        //                objOBLEntry.ImporterAddress = Convert.ToString(Result.Tables[0].Rows[0]["ImporterAddress"]);
        //                objOBLEntry.ImporterAddress1 = Convert.ToString(Result.Tables[0].Rows[0]["ImporterAddress1"]);
        //                objOBLEntry.CargoType = Convert.ToInt32(Result.Tables[0].Rows[0]["CargoType"]);
        //                objOBLEntry.SMTPNo = Convert.ToString(Result.Tables[0].Rows[0]["SMTPNo"]);
        //                objOBLEntry.TSANo = Convert.ToString(Result.Tables[0].Rows[0]["TSANo"]);
        //                objOBLEntry.TSADate = Convert.ToString(Result.Tables[0].Rows[0]["TSADate"]);
        //                objOBLEntry.BOEDate = Convert.ToString(Result.Tables[0].Rows[0]["BOEDate"]);
        //            }

        //            foreach (DataRow dr in Result.Tables[1].Rows)
        //            {
        //                Ppg_OBLWiseContainerEntryDetails objOBLEntryDetails = new Ppg_OBLWiseContainerEntryDetails();
        //                objOBLEntryDetails.ShippingLineName = Convert.ToString(dr["ShippingLineName"]);
        //                objOBLEntry.ShippingLine = objOBLEntryDetails.ShippingLineName;
        //                objOBLEntryDetails.ShippingLineId = Convert.ToInt32(dr["ShippingLineId"]);
        //                objOBLEntryDetails.ContainerSize = Convert.ToString(dr["ContainerSize"]);
        //                objOBLEntryDetails.ContainerNo = Convert.ToString(dr["ContainerNo"]);
        //                objOBLEntryDetails.NoOfPkg = Convert.ToString(dr["NO_PKG"]);
        //                objOBLEntryDetails.GR_WT = Convert.ToDecimal(dr["GR_WT"]);
        //                objOBLEntry.OblEntryDetailsList.Add(objOBLEntryDetails);
        //            }


        //            if (Result.Tables[2].Rows.Count > 0)
        //            {
        //                objOBLEntry.DestuffingNo = Convert.ToString(Result.Tables[2].Rows[0]["DestuffingNo"]);
        //                objOBLEntry.SQM = Convert.ToDecimal(Result.Tables[2].Rows[0]["SQM"]);
        //                objOBLEntry.CBM = Convert.ToDecimal(Result.Tables[2].Rows[0]["CBM"]);
        //                objOBLEntry.GodownNo = Convert.ToString(Result.Tables[2].Rows[0]["GodownName"]);
        //                objOBLEntry.Location = Convert.ToString(Result.Tables[2].Rows[0]["Location"]);
        //                //objOBLEntry.InvNo = Convert.ToString(Result.Tables[2].Rows[0]["Invoiceno"]);
        //                //objOBLEntry.InvDate = Convert.ToString(Result.Tables[2].Rows[0]["InvoiceDate"]);
        //                //objOBLEntry.GatePassNo = Convert.ToString(Result.Tables[2].Rows[0]["GatePassNo"]);
        //                //objOBLEntry.GatePassDate = Convert.ToString(Result.Tables[2].Rows[0]["GatePassDateTime"].ToString());
        //                //objOBLEntry.GateExitNo = Convert.ToString(Result.Tables[2].Rows[0]["GateExitNo"]);
        //                //objOBLEntry.GateExitDate = Convert.ToString(Result.Tables[2].Rows[0]["GateExitDateTime"].ToString());
        //            }
        //            if (Result.Tables[3].Rows.Count > 0)
        //            {
        //                //objOBLEntry.DestuffingNo = Convert.ToString(Result.Tables[2].Rows[0]["DestuffingNo"]);
        //                //objOBLEntry.SQM = Convert.ToDecimal(Result.Tables[2].Rows[0]["SQM"]);
        //                //objOBLEntry.CBM = Convert.ToDecimal(Result.Tables[2].Rows[0]["CBM"]);
        //                //objOBLEntry.GodownNo = Convert.ToString(Result.Tables[2].Rows[0]["GodownName"]);
        //                //objOBLEntry.Location = Convert.ToString(Result.Tables[2].Rows[0]["Location"]);
        //                objOBLEntry.InvNo = Convert.ToString(Result.Tables[3].Rows[0]["Invoiceno"]);
        //                objOBLEntry.InvDate = Convert.ToString(Result.Tables[3].Rows[0]["InvoiceDate"]);
        //                objOBLEntry.GatePassNo = Convert.ToString(Result.Tables[3].Rows[0]["GatePassNo"]);
        //                objOBLEntry.GatePassDate = Convert.ToString(Result.Tables[3].Rows[0]["GatePassDateTime"].ToString());
        //                objOBLEntry.GateExitNo = Convert.ToString(Result.Tables[3].Rows[0]["GateExitNo"]);
        //                objOBLEntry.GateExitDate = Convert.ToString(Result.Tables[3].Rows[0]["GateExitDateTime"].ToString());
        //            }
        //        }

        //        if (Status == 1)
        //        {
        //            //if (OblEntryDetailsList.Count > 0)
        //            //{
        //            //    objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(OblEntryDetailsList);
        //            //}

        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = objOBLEntry;
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
        //    }
        //}

        //public void GetICEGateTSADetail(string TSANo, string OBLNo, string SearchBy)
        //{
        //    //DateTime dtobl = DateTime.ParseExact(obldate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    //string obldt = dtobl.ToString("yyyy/MM/dd");
        //    DataSet Result = new DataSet();
        //    int Status = 0;
        //    try
        //    {
        //        //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //        IDataParameter[] DParam = { };
        //        List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //        DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //        LstParam.Add(new MySqlParameter { ParameterName = "In_TSANo", MySqlDbType = MySqlDbType.String, Value = TSANo });
        //        LstParam.Add(new MySqlParameter { ParameterName = "In_OBLNo", MySqlDbType = MySqlDbType.String, Value = OBLNo });
        //        // LstParam.Add(new MySqlParameter { ParameterName = "In_OBLDate", MySqlDbType = MySqlDbType.DateTime, Value = obldt });
        //        LstParam.Add(new MySqlParameter { ParameterName = "In_SearchBy", MySqlDbType = MySqlDbType.String, Value = SearchBy });
        //        DParam = LstParam.ToArray();
        //        Result = DataAccess.ExecuteDataSet("TSAQuery", CommandType.StoredProcedure, DParam);

        //        _DBResponse = new DatabaseResponse();

        //        Ppg_TSAQuery objOBLEntry = new Ppg_TSAQuery();

        //        //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();

        //        if (Result != null && Result.Tables.Count > 0)
        //        {
        //            Status = 1;
        //            if (Result.Tables[0].Rows.Count > 0)
        //            {
        //                objOBLEntry.OBL_No = Convert.ToString(Result.Tables[0].Rows[0]["OBL_NO"]);
        //                objOBLEntry.OBL_Date = Convert.ToString(Result.Tables[0].Rows[0]["OBL_DATE"]);
        //                objOBLEntry.IGM_No = Convert.ToString(Result.Tables[0].Rows[0]["IGM_NO"]);
        //                objOBLEntry.IGM_Date = Convert.ToString(Result.Tables[0].Rows[0]["IGM_DATE"]);
        //                objOBLEntry.TPNo = Convert.ToString(Result.Tables[0].Rows[0]["TP_NO"]);
        //                objOBLEntry.TPDate = Convert.ToString(Result.Tables[0].Rows[0]["TP_DATE"]);
        //                objOBLEntry.MovementType = Convert.ToString(Result.Tables[0].Rows[0]["MovementType"]);
        //                objOBLEntry.PortId = Convert.ToInt32(Result.Tables[0].Rows[0]["PortId"]);
        //                objOBLEntry.LineNo = Convert.ToString(Result.Tables[0].Rows[0]["LINE_NO"].ToString());
        //                objOBLEntry.CargoDescription = Convert.ToString(Result.Tables[0].Rows[0]["CargoDescription"]);
        //                objOBLEntry.NoOfPkg = Convert.ToString(Result.Tables[0].Rows[0]["NO_PKG"]);
        //                objOBLEntry.PkgType = Convert.ToString(Result.Tables[0].Rows[0]["PKG_TYPE"]);
        //                objOBLEntry.GR_WT = Convert.ToDecimal(Result.Tables[0].Rows[0]["GR_WT"]);
        //                objOBLEntry.ImporterId = Convert.ToInt32(Result.Tables[0].Rows[0]["ImporterId"]);
        //                objOBLEntry.ImporterName = Convert.ToString(Result.Tables[0].Rows[0]["ImporterName"]);
        //                objOBLEntry.ImporterAddress = Convert.ToString(Result.Tables[0].Rows[0]["ImporterAddress"]);
        //                objOBLEntry.ImporterAddress1 = Convert.ToString(Result.Tables[0].Rows[0]["ImporterAddress1"]);
        //                objOBLEntry.CargoType = Convert.ToInt32(Result.Tables[0].Rows[0]["CargoType"]);
        //                objOBLEntry.SMTPNo = Convert.ToString(Result.Tables[0].Rows[0]["SMTPNo"]);
        //                objOBLEntry.TSANo = Convert.ToString(Result.Tables[0].Rows[0]["TSANo"]);
        //                objOBLEntry.TSADate = Convert.ToString(Result.Tables[0].Rows[0]["TSADate"]);
        //            }

        //            foreach (DataRow dr in Result.Tables[1].Rows)
        //            {
        //                Ppg_OBLWiseContainerEntryDetails objOBLEntryDetails = new Ppg_OBLWiseContainerEntryDetails();
        //                objOBLEntryDetails.ShippingLineName = Convert.ToString(dr["ShippingLineName"]);
        //                objOBLEntry.ShippingLine = objOBLEntryDetails.ShippingLineName;
        //                objOBLEntryDetails.ShippingLineId = Convert.ToInt32(dr["ShippingLineId"]);
        //                objOBLEntryDetails.ContainerSize = Convert.ToString(dr["ContainerSize"]);
        //                objOBLEntryDetails.ContainerNo = Convert.ToString(dr["ContainerNo"]);
        //                objOBLEntryDetails.NoOfPkg = Convert.ToString(dr["NO_PKG"]);
        //                objOBLEntryDetails.GR_WT = Convert.ToDecimal(dr["GR_WT"]);
        //                objOBLEntry.OblEntryDetailsList.Add(objOBLEntryDetails);
        //            }

        //            if (Result.Tables[2].Rows.Count > 0)
        //            {
        //                objOBLEntry.DestuffingNo = Convert.ToString(Result.Tables[2].Rows[0]["DestuffingNo"]);
        //                objOBLEntry.DestuffingDate = Convert.ToString(Result.Tables[2].Rows[0]["DestuffDate"]);
        //                objOBLEntry.SQM = Convert.ToDecimal(Result.Tables[2].Rows[0]["SQM"]);
        //                objOBLEntry.CBM = Convert.ToDecimal(Result.Tables[2].Rows[0]["CBM"]);
        //                objOBLEntry.GodownNo = Convert.ToString(Result.Tables[2].Rows[0]["GodownName"]);
        //                objOBLEntry.Location = Convert.ToString(Result.Tables[2].Rows[0]["Location"]);
        //                // objOBLEntry.DestuffingDate = Convert.ToString(Result.Tables[2].Rows[0]["DestuffDate"]);
        //                //objOBLEntry.InvNo = Convert.ToString(Result.Tables[2].Rows[0]["Invoiceno"]);
        //                //objOBLEntry.InvDate = Convert.ToString(Result.Tables[2].Rows[0]["InvoiceDate"]);
        //                //objOBLEntry.GatePassNo = Convert.ToString(Result.Tables[2].Rows[0]["GatePassNo"]);
        //                //objOBLEntry.GatePassDate = Convert.ToString(Result.Tables[2].Rows[0]["GatePassDateTime"].ToString());
        //                //objOBLEntry.GateExitNo = Convert.ToString(Result.Tables[2].Rows[0]["GateExitNo"]);
        //                //objOBLEntry.GateExitDate = Convert.ToString(Result.Tables[2].Rows[0]["GateExitDateTime"].ToString());
        //            }
        //            if (Result.Tables[3].Rows.Count > 0)
        //            {
        //                //objOBLEntry.DestuffingNo = Convert.ToString(Result.Tables[2].Rows[0]["DestuffingNo"]);
        //                //objOBLEntry.SQM = Convert.ToDecimal(Result.Tables[2].Rows[0]["SQM"]);
        //                //objOBLEntry.CBM = Convert.ToDecimal(Result.Tables[2].Rows[0]["CBM"]);
        //                //objOBLEntry.GodownNo = Convert.ToString(Result.Tables[2].Rows[0]["GodownName"]);
        //                //objOBLEntry.Location = Convert.ToString(Result.Tables[2].Rows[0]["Location"]);
        //                objOBLEntry.InvNo = Convert.ToString(Result.Tables[3].Rows[0]["Invoiceno"]);
        //                objOBLEntry.InvDate = Convert.ToString(Result.Tables[3].Rows[0]["InvoiceDate"]);
        //                objOBLEntry.GatePassNo = Convert.ToString(Result.Tables[3].Rows[0]["GatePassNo"]);
        //                objOBLEntry.GatePassDate = Convert.ToString(Result.Tables[3].Rows[0]["GatePassDateTime"].ToString());
        //                objOBLEntry.GateExitNo = Convert.ToString(Result.Tables[3].Rows[0]["GateExitNo"]);
        //                objOBLEntry.GateExitDate = Convert.ToString(Result.Tables[3].Rows[0]["GateExitDateTime"].ToString());
        //            }
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = objOBLEntry;
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
        //    }
        //}
        //#endregion

        #region SBQueryReport
        public void GetAllSBForExternalUser(int PartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShipBillListForSBQueryForExternalUser", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_SBQuery> LstSB = new List<PPG_SBQuery>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new PPG_SBQuery
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        SBNODate = Result["SBNODate"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
                }
                else
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

        public void SBQueryReportExUser(int id, string sbno, string sbdate)
        {


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBNO", MySqlDbType = MySqlDbType.VarChar, Value = sbno });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBDate", MySqlDbType = MySqlDbType.Date, Value = sbdate });

            int Status = 0;

            IDataParameter[] DParam = { };



            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSBQueryDetailsExUser", CommandType.StoredProcedure, DParam);
            PPG_SBQuery LstSBQueryReport = new PPG_SBQuery();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSBQueryReport.Id = Convert.ToInt32(Result["Id"]);
                    LstSBQueryReport.SBNO = Convert.ToString(Result["SBNO"]);
                    LstSBQueryReport.PortOFDischarge = Convert.ToString(Result["PortOfDischarge"]);
                    LstSBQueryReport.PortOFLoad = Convert.ToString(Result["PortName"]);
                    LstSBQueryReport.ShippingLine = Convert.ToString(Result["ShippingLine"]);
                    LstSBQueryReport.Comodity = Convert.ToString(Result["Comodity"]);
                    LstSBQueryReport.CHA = Convert.ToString(Result["CHA"]);
                    LstSBQueryReport.Date = Convert.ToString(Result["Date"]);
                    //LstSBQueryReport.PortOFDischarge = Convert.ToString(Result["Id"]);
                    LstSBQueryReport.Package = Convert.ToInt32(Result["Package"]);
                    LstSBQueryReport.Weight = Convert.ToDecimal(Result["Weight"]);
                    LstSBQueryReport.FOB = Convert.ToDecimal(Result["FOB"]);
                    LstSBQueryReport.Cargotype = Convert.ToInt32(Result["CargoType"]);
                    //LstSBQueryReport.Vehicle = Convert.ToString(Result["NoOfVehicle"]);
                    LstSBQueryReport.Exporter = Convert.ToString(Result["Exporter"]);
                    LstSBQueryReport.Country = Convert.ToString(Result["Country"]);
                    LstSBQueryReport.GateinNo = Convert.ToString(Result["GateInNo"]);
                    // LstSBQueryReport.PCIN = Convert.ToString(Result["PCIN"]);
                    LstSBQueryReport.InvoiceNo = Convert.ToString(Result["InvoiceNo"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        LstSBQueryReport.CartingSBQList.Add(new PPG_CartingFORSB
                        {
                            Date = Convert.ToString(Result["Date"]),
                            CartingRegisterNo = Result["CartingRegisterNo"].ToString(),
                            Godown = Convert.ToString(Result["Godown"]),
                            Remarks = Convert.ToString(Result["Remarks"]),
                            Location = Result["Location"].ToString(),
                            NOOfPackages = Convert.ToInt32(Result["NoOFPackage"]),
                            ReserveCBM = Convert.ToDecimal(Result["ReserveCBM"]),
                            UnReserveCBM = Convert.ToDecimal(Result["UnReserveCBM"]),
                            excessbalancecargo = Convert.ToInt32(Result["excessbalancecargo"]),
                            ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                            InvoiceNo = Convert.ToString(Result["invoiceno"]),
                        });



                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        LstSBQueryReport.DeliverySBQList.Add(new PPG_DeliveryFORSBQuery
                        {
                            Date = Convert.ToString(Result["Date"]),
                            InvoiceNO = Result["InvoiceNo"].ToString(),
                            //  Exporter = Convert.ToString(Result["Exporter"]),
                            //  CHA = Result["CHA"].ToString(),
                            NOOfPackages = Convert.ToInt32(Result["NoOFPackage"]),
                            ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                            ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                            StuReqNo = Convert.ToString(Result["StuffingReqNo"]),
                            CfsCode = Convert.ToString(Result["cfscode"]),
                            Forwarder = Convert.ToString(Result["ForwarderName"]),
                            Transporter = Convert.ToString(Result["transportername"]),
                            SealNo = Convert.ToString(Result["SealNo"]),
                            SealDate = Convert.ToString(Result["SealDate"]),
                            IwbNo = Convert.ToString(Result["IWBNo"]),
                            ContainerNo = Convert.ToString(Result["containerno"]),
                            Size = Convert.ToString(Result["size"]),
                            PortOfLoading = Convert.ToString(Result["Portofloading"]),
                            VehicleNo = Convert.ToString(Result["VehicleNo"]),
                            OutDate = Convert.ToString(Result["outdate"]),







                        });


                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        LstSBQueryReport.BTTSBQList.Add(new PPG_BTTFORSBQuery
                        {
                            Date = Convert.ToString(Result["invDate"]),
                            InvoiceNO = Result["InvoiceNo"].ToString(),
                            NOOfPackages = Convert.ToInt32(Result["NoOfUnits"]),
                            ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                            ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        });



                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        LstSBQueryReport.StockSBQList.Add(new PPG_StockSBQuery
                        {

                            NOOfPackages = Convert.ToInt32(Result["Units"]),
                            SQM = Convert.ToDecimal(Result["Area"]),
                            Weight = Convert.ToDecimal(Result["Weight"]),
                        });



                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        LstSBQueryReport.Pallatisation.Add(new PPG_DeliveryFORSBQuery
                        {
                            Date = Convert.ToString(Result["Date"]),
                            InvoiceNO = Result["InvoiceNo"].ToString(),
                            Exporter = Convert.ToString(Result["Exporter"]),
                            CHA = Result["CHA"].ToString(),
                            NOOfPackages = Convert.ToInt32(Result["NoOFPackage"]),
                            NOOfPallet = Convert.ToInt32(Result["NoOFPallet"]),
                            ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                            ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        });



                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSBQueryReport;
                }
                else
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

        #region BEO Details External User

        public void ListOfBEONoForExtUser(string BEO, int PartyId, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BEO", MySqlDbType = MySqlDbType.VarChar, Value = BEO });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });

            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBOENoForExternalUser", CommandType.StoredProcedure, Dparam);
            IList<PpgOBLNoForPage> LstObl = new List<PpgOBLNoForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstObl.Add(new PpgOBLNoForPage
                    {
                        OBLNo = Result["BOE_NO"].ToString(),
                        LINENo = Result["BOE_DATE"].ToString(),
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
                    _DBResponse.Data = new { LstObl, State };
                }
                else
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

        public void GetBEODetails(string BEONo, string BEODate)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = BEONo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BEODate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(BEODate) });

            IDataParameter[] Dparam = lstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("GetBOENoDetails", CommandType.StoredProcedure, Dparam);
            PPG_BEODetails objBEODetails = new PPG_BEODetails();
            _DBResponse = new DatabaseResponse();
            try
            {
                Status = 1;
                objBEODetails.BeoNoDetails.BOE_NO = Convert.ToString(Result.Tables[0].Rows[0]["BOE_NO"]);
                objBEODetails.BeoNoDetails.BOE_DATE = Convert.ToString(Result.Tables[0].Rows[0]["BOE_DATE"]);

                objBEODetails.BeoNoDetails.IMP_NAME = Convert.ToString(Result.Tables[0].Rows[0]["IMP_NAME"]);
                objBEODetails.BeoNoDetails.IEC_CD = Convert.ToString(Result.Tables[0].Rows[0]["IEC_CD"]);

                objBEODetails.BeoNoDetails.CHA_CODE = Convert.ToString(Result.Tables[0].Rows[0]["CHA_CODE"]);
                objBEODetails.BeoNoDetails.CIF_VALUE = Convert.ToString(Result.Tables[0].Rows[0]["CIF_VALUE"]);
                objBEODetails.BeoNoDetails.CITY = Convert.ToString(Result.Tables[0].Rows[0]["CITY"]);
                objBEODetails.BeoNoDetails.CONTAINER_NO = Convert.ToString(Result.Tables[0].Rows[0]["CONTAINER_NO"]);
                objBEODetails.BeoNoDetails.C_OF_ORIGIN = Convert.ToString(Result.Tables[0].Rows[0]["C_OF_ORIGIN"]);
                objBEODetails.BeoNoDetails.DUTY = Convert.ToString(Result.Tables[0].Rows[0]["DUTY"]);
                objBEODetails.BeoNoDetails.IMP_ADD1 = Convert.ToString(Result.Tables[0].Rows[0]["IMP_ADD1"]);
                objBEODetails.BeoNoDetails.IMP_ADD2 = Convert.ToString(Result.Tables[0].Rows[0]["IMP_ADD2"]);
                objBEODetails.BeoNoDetails.PIN = Convert.ToString(Result.Tables[0].Rows[0]["PIN"]);

                objBEODetails.BeoNoDetails.NO_PKG = Convert.ToString(Result.Tables[0].Rows[0]["NO_PKG"]);
                objBEODetails.BeoNoDetails.GR_WT = Convert.ToString(Result.Tables[0].Rows[0]["GR_WT"]);
                objBEODetails.BeoNoDetails.PKG_TYPE = Convert.ToString(Result.Tables[0].Rows[0]["PKG_TYPE"]);
                objBEODetails.BeoNoDetails.UNIT_TYPE = Convert.ToString(Result.Tables[0].Rows[0]["UNIT_TYPE"]);

                if (Result.Tables[1].Rows.Count > 0)
                {
                    objBEODetails.BeoOOCDetails.CIF_VALUE = Convert.ToString(Result.Tables[1].Rows[0]["CIF_VALUE"]);
                    objBEODetails.BeoOOCDetails.DUTY = Convert.ToString(Result.Tables[1].Rows[0]["DUTY"]);
                    objBEODetails.BeoOOCDetails.GR_WT = Convert.ToString(Result.Tables[1].Rows[0]["GR_WT"]);
                    objBEODetails.BeoOOCDetails.NO_PKG = Convert.ToString(Result.Tables[1].Rows[0]["NO_PKG"]);
                    objBEODetails.BeoOOCDetails.OOC_DATE = Convert.ToString(Result.Tables[1].Rows[0]["OOC_DATE"]);
                    objBEODetails.BeoOOCDetails.OOC_NO = Convert.ToString(Result.Tables[1].Rows[0]["OOC_NO"]);
                    objBEODetails.BeoOOCDetails.OOC_TYPE = Convert.ToString(Result.Tables[1].Rows[0]["OOC_TYPE"]);
                    objBEODetails.BeoOOCDetails.PKG_TYPE = Convert.ToString(Result.Tables[1].Rows[0]["PKG_TYPE"]);
                    objBEODetails.BeoOOCDetails.UNIT_TYPE = Convert.ToString(Result.Tables[1].Rows[0]["UNIT_TYPE"]);
                }


                foreach (DataRow dr in Result.Tables[2].Rows)
                {
                    objBEODetails.BeoIGMDetails.Add(new PPG_BeoIGMDetails
                    {
                        CARGO_DESC = Convert.ToString(dr["CARGO_DESC"]),
                        CONTAINER_NO = Convert.ToString(dr["CONTAINER_NO"]),
                        GR_WT = Convert.ToString(dr["GR_WT"]),
                        ITEM_TYPE = Convert.ToString(dr["ITEM_TYPE"]),
                        LINE_NO = Convert.ToString(dr["LINE_NO"]),
                        LOCAL_IGM_DATE = Convert.ToString(dr["LOCAL_IGM_DATE"]),
                        LOCAL_IGM_NO = Convert.ToString(dr["LOCAL_IGM_NO"]),
                        NATURE_OF_CARGO = Convert.ToString(dr["NATURE_OF_CARGO"]),
                        NO_PKG = Convert.ToString(dr["NO_PKG"]),
                        OBL_DATE = Convert.ToString(dr["OBL_DATE"]),
                        OBL_NO = Convert.ToString(dr["OBL_NO"]),
                        PKG_TYPE = Convert.ToString(dr["PKG_TYPE"]),
                        PORT_OF_LOADING = Convert.ToString(dr["PORT_OF_LOADING"]),
                        SUB_LINE_NO = Convert.ToString(dr["SUB_LINE_NO"]),
                        UNIT_OF_VOLUME = Convert.ToString(dr["UNIT_OF_VOLUME"]),
                        UNIT_OF_WEIGHT = Convert.ToString(dr["UNIT_OF_WEIGHT"]),
                        VOLUME = Convert.ToString(dr["VOLUME"]),


                    });
                }




                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objBEODetails;
                }
                else
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
                //  Result.Dispose();
                //Result.Close();
            }

        }
        #endregion

        #region TDS receipt 

        public void GetTDSReceiptListForExternalUser(string FromDate, string ToDate, int UserID)
        {
            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = UserID });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("TDSReceiptListWithDateForExternalUser", CommandType.StoredProcedure, DParam);
            IList<PPG_ReceiptList> LstReceiptList = new List<PPG_ReceiptList>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstReceiptList.Add(new PPG_ReceiptList
                    {
                        ReceiptNumber = Result["ReceiptNo"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = JsonConvert.SerializeObject(LstReceiptList); ;
                }
                else
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

        public void GetTDSreceipt(string ReceiptNo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate == null ? "" : FromDate).ToString("yyyy-MM-dd") });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate == null ? "" : ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ReceiptNo });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetTDSRecptForPrint", CommandType.StoredProcedure, DParam);
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

        #region DELIVERY Report For External User
        public void DeliveryReportForExternalUser(PPG_DeliveryRpt vm, int PartyId)
        {

            DateTime dtfrom = DateTime.ParseExact(vm.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(vm.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DeliveryReportForExternalUser", CommandType.StoredProcedure, DParam);
            IList<PPG_DeliveryRpt> lstData = new List<PPG_DeliveryRpt>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstData.Add(new PPG_DeliveryRpt
                    {

                        OBLNo = Result["BOLNo"].ToString(),
                        OBLDate = Result["BOLDate"].ToString(),
                        TSANo = Result["TSANo"].ToString(),
                        TSADate = Result["TSA_Date"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        Importer = Result["Importer"].ToString(),
                        LineNo = Result["LineNo"].ToString(),
                        PKG = Convert.ToDecimal(Result["PKG"]),
                        WT = Convert.ToDecimal(Result["WT"]),
                        SLOT = Result["GodownName"].ToString(),
                        Area = Convert.ToDecimal(Result["SQM"]),
                        CBM = Convert.ToDecimal(Result["CBM"]),
                        Description = Result["CargoDescription"].ToString(),
                        GatePassNo = Result["GatePassNo"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        SLA = Result["SLA"].ToString(),
                        CHA = Result["ChaName"].ToString(),
                        DAYS = Convert.ToInt32(Result["Days"]),
                        AmountReceived = Convert.ToDecimal(Result["StorageCharge"]),
                        CIFValue = Convert.ToDecimal(Result["CIF"]),
                        DUTY = Convert.ToDecimal(Result["DUTY"])
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstData;
                }
                else
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

        #region DESTUFFING Daily Report For External User
        public void DestuffingDetailReportForExternalUser(Ppg_DestuffingDailyReport vm, int EximTraderId)
        {

            DateTime dtfrom = DateTime.ParseExact(vm.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(vm.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = EximTraderId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDeStuffingReportForExternalUser", CommandType.StoredProcedure, DParam);
            IList<Ppg_DestuffingDailyReport> lstData = new List<Ppg_DestuffingDailyReport>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstData.Add(new Ppg_DestuffingDailyReport
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        PortName = Result["PortName"].ToString(),
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        // Weight = Result["Weight"].ToString(),
                        //ImportExport = Result["OperationType"].ToString(),
                        //TotalAmount = Convert.ToDecimal(Result["TotalAmount"]),
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstData;
                }
                else
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

        #region Stock Position Report For External User
        public void StockRegisterReportForExternalUser(PPG_StockRegisterReport vm)
        {

            //DateTime dtfrom = DateTime.ParseExact(vm.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(vm.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = vm.ShippingLineId });
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("StockRegisterReportForExternalUser", CommandType.StoredProcedure, DParam);
            List<PPG_StockRegisterReport> lstStockRegisterReport = new List<PPG_StockRegisterReport>();
            _DBResponse = new DatabaseResponse();
            try
            {
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    Status = 1;
                    lstStockRegisterReport.Add(new PPG_StockRegisterReport
                    {
                        Area = Convert.ToDecimal(dr["Area"]),
                        CBM = Convert.ToDecimal(dr["CBM"]),
                        CFSCode = Convert.ToString(dr["CFSCode"]),
                        CIF = Convert.ToDecimal(dr["CIFValue"]),
                        ContainerNo = Convert.ToString(dr["ContainerNo"]),
                        DestuffDate = Convert.ToString(dr["DestuffingEntryDate"]),
                        HAZNoHAZ = Convert.ToString(dr["CargoType"]),
                        Importer = Convert.ToString(dr["ImporterName"]),
                        OBLDate = Convert.ToString(dr["BOLDate"]),
                        OBLNo = Convert.ToString(dr["BOLNo"]),
                        ContNo = Convert.ToString(dr["ContNo"]),
                        ArrivalDate = Convert.ToString(dr["ArrivalDate"]),
                        Pkg = Convert.ToDecimal(dr["NoOfUnits"]),
                        SLA = Convert.ToString(dr["SLA"]),
                        Slot = Convert.ToString(dr["LocationName"]),
                        StoreCharge = Convert.ToDecimal(dr["StoreCharge"]),
                        TSADate = Convert.ToString(dr["TSADate"]),
                        TSANo = Convert.ToString(dr["TSANo"]),
                        SMTPDate = Convert.ToString(dr["SMTPDate"]),
                        SMTPNo = Convert.ToString(dr["SMTPNo"]),
                        WT = Convert.ToDecimal(dr["Weight"]),
                        Days = Convert.ToInt32(dr["Days"]),
                        DESC = Convert.ToString(dr["CommodityAlias"])

                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstStockRegisterReport;
                }
                else
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

        }

        public void GetShippingLineForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page }); IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForPage", CommandType.StoredProcedure, DParam);
            List<PPG_ShippingLineForPage> LstShippingLine = new List<PPG_ShippingLineForPage>();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstShippingLine.Add(new PPG_ShippingLineForPage
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Result["ShippingLine"].ToString(),
                        PartyCode = Convert.ToString(Result["PartyCode"]),
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),

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
                    _DBResponse.Data = new { LstShippingLine, State };
                }
                else
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

        #region ICE Gate Detail for External User
        public void ListOfICEGateOBLNoForExternal(string SearchBy, string OBLNo, int PartyId, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLNo", MySqlDbType = MySqlDbType.VarChar, Value = OBLNo });            
            lstParam.Add(new MySqlParameter { ParameterName = "In_SearchBy", MySqlDbType = MySqlDbType.VarChar, Value = SearchBy });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLNoForOBLSearchForExternalUser", CommandType.StoredProcedure, Dparam);
            IList<OBLNoForPages> LstObl = new List<OBLNoForPages>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstObl.Add(new OBLNoForPages
                    {
                        OBLNo = Result["OBLNo"].ToString()
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
                    _DBResponse.Data = new { LstObl, State };
                }
                else
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

        #region ICE Details

         public void ListOfICEGateOBLNo(string SearchBy, string OBLNo, int Page = 0)
         {
             int Status = 0;
             DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
             List<MySqlParameter> lstParam = new List<MySqlParameter>();
             lstParam.Add(new MySqlParameter { ParameterName = "in_OBLNo", MySqlDbType = MySqlDbType.VarChar, Value = OBLNo });
             lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
             lstParam.Add(new MySqlParameter { ParameterName = "In_SearchBy", MySqlDbType = MySqlDbType.VarChar, Value = SearchBy });
             IDataParameter[] Dparam = lstParam.ToArray();
             IDataReader Result = DataAccess.ExecuteDataReader("GetOBLNoForOBLSearchForExternalUser", CommandType.StoredProcedure, Dparam);
             IList<OBLNoForPages> LstObl = new List<OBLNoForPages>();
             _DBResponse = new DatabaseResponse();
             try
             {
                 bool State = false;
                 while (Result.Read())
                 {
                     Status = 1;
                     LstObl.Add(new OBLNoForPages
                     {
                         OBLNo = Result["OBLNo"].ToString()
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
                     _DBResponse.Data = new { LstObl, State };
                 }
                 else
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
        

        public void GetICEGateDetail(string oblnum, string obldate, string SearchBy)
        {
            DateTime dtobl = DateTime.ParseExact(obldate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string obldt = dtobl.ToString("yyyy/MM/dd");
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLNo", MySqlDbType = MySqlDbType.String, Value = oblnum });
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLDate", MySqlDbType = MySqlDbType.DateTime, Value = obldt });
                LstParam.Add(new MySqlParameter { ParameterName = "In_SearchBy", MySqlDbType = MySqlDbType.String, Value = SearchBy });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetICEGateData", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                Ppg_OBLWiseContainerEntry objOBLEntry = new Ppg_OBLWiseContainerEntry();

                //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        objOBLEntry.OBL_No = Convert.ToString(Result.Tables[0].Rows[0]["OBL_NO"]);
                        objOBLEntry.OBL_Date = Convert.ToString(Result.Tables[0].Rows[0]["OBL_DATE"]);
                        objOBLEntry.IGM_No = Convert.ToString(Result.Tables[0].Rows[0]["IGM_NO"]);
                        objOBLEntry.IGM_Date = Convert.ToString(Result.Tables[0].Rows[0]["IGM_DATE"]);
                        objOBLEntry.TPNo = Convert.ToString(Result.Tables[0].Rows[0]["TP_NO"]);
                        objOBLEntry.TPDate = Convert.ToString(Result.Tables[0].Rows[0]["TP_DATE"]);
                        objOBLEntry.MovementType = Convert.ToString(Result.Tables[0].Rows[0]["MovementType"]);
                        objOBLEntry.PortId = Convert.ToInt32(Result.Tables[0].Rows[0]["PortId"]);
                        objOBLEntry.LineNo = Convert.ToString(Result.Tables[0].Rows[0]["LINE_NO"].ToString());
                        objOBLEntry.CargoDescription = Convert.ToString(Result.Tables[0].Rows[0]["CargoDescription"]);
                        objOBLEntry.NoOfPkg = Convert.ToString(Result.Tables[0].Rows[0]["NO_PKG"]);
                        objOBLEntry.PkgType = Convert.ToString(Result.Tables[0].Rows[0]["PKG_TYPE"]);
                        objOBLEntry.GR_WT = Convert.ToDecimal(Result.Tables[0].Rows[0]["GR_WT"]);
                        objOBLEntry.ImporterId = Convert.ToInt32(Result.Tables[0].Rows[0]["ImporterId"]);
                        objOBLEntry.ImporterName = Convert.ToString(Result.Tables[0].Rows[0]["ImporterName"]);
                        objOBLEntry.ImporterAddress = Convert.ToString(Result.Tables[0].Rows[0]["ImporterAddress"]);
                        objOBLEntry.ImporterAddress1 = Convert.ToString(Result.Tables[0].Rows[0]["ImporterAddress1"]);
                        objOBLEntry.CargoType = Convert.ToInt32(Result.Tables[0].Rows[0]["CargoType"]);
                        objOBLEntry.SMTPNo = Convert.ToString(Result.Tables[0].Rows[0]["SMTPNo"]);
                        //objOBLEntry.TSANo = Convert.ToString(Result.Tables[0].Rows[0]["TSANo"]);
                        //objOBLEntry.TSADate = Convert.ToString(Result.Tables[0].Rows[0]["TSADate"]);
                    }

                    //foreach (DataRow dr in Result.Tables[1].Rows)
                    //{
                    //    Ppg_OBLWiseContainerEntry objOBLEntryDetails = new Ppg_OBLWiseContainerEntry();
                    //    objOBLEntryDetails.ShippingLineName = Convert.ToString(dr["ShippingLineName"]);
                    //    objOBLEntry.ShippingLine = objOBLEntryDetails.ShippingLineName;
                    //    objOBLEntryDetails.ShippingLineId = Convert.ToInt32(dr["ShippingLineId"]);
                    //    objOBLEntryDetails.ContainerSize = Convert.ToString(dr["ContainerSize"]);
                    //    objOBLEntryDetails.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                    //    objOBLEntryDetails.NoOfPkg = Convert.ToString(dr["NO_PKG"]);
                    //    objOBLEntryDetails.GR_WT = Convert.ToDecimal(dr["GR_WT"]);
                    //    objOBLEntry.OblEntryDetailsList.Add(objOBLEntryDetails);
                    //}

                    //if (Result.Tables[2].Rows.Count > 0)
                    //{
                    //    objOBLEntry.DestuffingNo = Convert.ToString(Result.Tables[2].Rows[0]["DestuffingNo"]);
                    //    objOBLEntry.DestuffingDate = Convert.ToString(Result.Tables[2].Rows[0]["DestuffingDate"]);
                    //    objOBLEntry.SQM = Convert.ToDecimal(Result.Tables[2].Rows[0]["SQM"]);
                    //    objOBLEntry.CBM = Convert.ToDecimal(Result.Tables[2].Rows[0]["CBM"]);
                    //    objOBLEntry.GodownNo = Convert.ToString(Result.Tables[2].Rows[0]["GodownName"]);
                    //    objOBLEntry.Location = Convert.ToString(Result.Tables[2].Rows[0]["Location"]);
                    //    //objOBLEntry.InvNo = Convert.ToString(Result.Tables[2].Rows[0]["Invoiceno"]);
                    //    //objOBLEntry.InvDate = Convert.ToString(Result.Tables[2].Rows[0]["InvoiceDate"]);
                    //    //objOBLEntry.GatePassNo = Convert.ToString(Result.Tables[2].Rows[0]["GatePassNo"]);
                    //    //objOBLEntry.GatePassDate = Convert.ToString(Result.Tables[2].Rows[0]["GatePassDateTime"].ToString());
                    //    //objOBLEntry.GateExitNo = Convert.ToString(Result.Tables[2].Rows[0]["GateExitNo"]);
                    //    //objOBLEntry.GateExitDate = Convert.ToString(Result.Tables[2].Rows[0]["GateExitDateTime"].ToString());
                    //}
                    //if (Result.Tables[3].Rows.Count > 0)
                    //{
                    //    //objOBLEntry.DestuffingNo = Convert.ToString(Result.Tables[2].Rows[0]["DestuffingNo"]);
                    //    //objOBLEntry.SQM = Convert.ToDecimal(Result.Tables[2].Rows[0]["SQM"]);
                    //    //objOBLEntry.CBM = Convert.ToDecimal(Result.Tables[2].Rows[0]["CBM"]);
                    //    //objOBLEntry.GodownNo = Convert.ToString(Result.Tables[2].Rows[0]["GodownName"]);
                    //    //objOBLEntry.Location = Convert.ToString(Result.Tables[2].Rows[0]["Location"]);
                    //    objOBLEntry.InvNo = Convert.ToString(Result.Tables[3].Rows[0]["Invoiceno"]);
                    //    objOBLEntry.InvDate = Convert.ToString(Result.Tables[3].Rows[0]["InvoiceDate"]);
                    //    objOBLEntry.GatePassNo = Convert.ToString(Result.Tables[3].Rows[0]["GatePassNo"]);
                    //    objOBLEntry.GatePassDate = Convert.ToString(Result.Tables[3].Rows[0]["GatePassDateTime"].ToString());
                    //    objOBLEntry.GateExitNo = Convert.ToString(Result.Tables[3].Rows[0]["GateExitNo"]);
                    //    objOBLEntry.GateExitDate = Convert.ToString(Result.Tables[3].Rows[0]["GateExitDateTime"].ToString());
                    //}
                }

                if (Status == 1)
                {
                    //if (OblEntryDetailsList.Count > 0)
                    //{
                    //    objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(OblEntryDetailsList);
                    //}

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objOBLEntry;
                }
                else
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

        public void DeleteOBLWiseContainer(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("DeleteOBLWiseContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Cannot Delete As It Exists In Seal Cutting";
                    _DBResponse.Status = 2;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Cannot Delete  As It Exists In Job Order By Train";
                    _DBResponse.Status = 3;
                }
                //else if (Result == -1)
                //{
                //    _DBResponse.Data = null;
                //    _DBResponse.Message = "Cannot Delete As It Exists In Another Page";
                //    _DBResponse.Status = -1;
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
    }
}