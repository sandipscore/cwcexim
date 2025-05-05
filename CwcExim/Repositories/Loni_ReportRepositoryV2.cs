using CwcExim.Areas.Auction.Models;
using CwcExim.Areas.Report.Models;
using CwcExim.DAL;
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
    public class Loni_ReportRepositoryV2
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

    }
}