using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using System.Data;
using MySql.Data.MySqlClient;
using CwcExim.Areas.Export.Models;
using System.Configuration;
using CwcExim.Models;
using CwcExim.Areas.Import.Models;
using System.Globalization;
using CwcExim.Areas.Report.Models;
using Newtonsoft.Json;

namespace CwcExim.Repositories
{
    public class CHN_ExtReportRepository
    { 

        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void GetPDAStatement( int Uid ,int Month, int Year)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.Int32, Value = Month });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = Year });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("ExtSdStatement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            ChnSDStatement ObjSDStatement = new ChnSDStatement();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        ObjSDStatement.LstSD.Add(new ChnSDList
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
        public void GetInvoiceList(string FromDate, string ToDate, string invoiceType,int Uid)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = invoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 10, Value = Uid });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("EximInvoiceListWithDate", CommandType.StoredProcedure, DParam);
            IList<BulkInvoiceReport> LstInvoice = new List<BulkInvoiceReport>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstInvoice.Add(new BulkInvoiceReport
                    {



                        InvoiceNumber = Result["InvoiceNumber"].ToString(),
                        PartyId= Convert.ToInt32(Result["PartyId"])//.ToString()




                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = JsonConvert.SerializeObject(LstInvoice);
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

    }
}