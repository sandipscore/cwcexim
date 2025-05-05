using System;
using System.Collections.Generic;
using System.Linq;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using CwcExim.Models;
using System.Data;
using CwcExim.Areas.Import.Models;
using System.Web;
using System.Globalization;
using CwcExim.Areas.GateOperation.Models;
using System.Data.Common;

namespace CwcExim.Repositories
{
    //For SRS Version 3.2 
    public class Ppg_ImportRepositoryV2
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        #region Train Summary TKD
        public void GetPortOfLoading()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortOfLoadingV2", CommandType.StoredProcedure);
            List<Port> LstPort = new List<Port>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new Port
                    {
                        PortName = Convert.ToString(Result["PortName"]),
                        PortId = Convert.ToInt32(Result["PortId"]),
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
        public void AddUpdateTrainSummaryUploadTKD(Ppg_TrainSummaryUploadV2 Obj,string SEZ)
        {
            try
            {
                Obj.TrainDate = DateTime.ParseExact(Obj.TrainDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                string GeneratedClientId = "";
                List<MySqlParameter> LstParam = new List<MySqlParameter>();

                LstParam.Add(new MySqlParameter { ParameterName = "in_WagonNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Wagon_No });
                LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Container_No });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = Obj.CT_Size });
                LstParam.Add(new MySqlParameter { ParameterName = "in_LineSealNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Line_Seal_No });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Commodity", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Cont_Commodity });
                LstParam.Add(new MySqlParameter { ParameterName = "in_SLine", MySqlDbType = MySqlDbType.VarChar, Value = Obj.S_Line });

                LstParam.Add(new MySqlParameter { ParameterName = "in_CtTare", MySqlDbType = MySqlDbType.Decimal, Value = Obj.Ct_Tare });
                LstParam.Add(new MySqlParameter { ParameterName = "in_CargoWt", MySqlDbType = MySqlDbType.Decimal, Value = Obj.Cargo_Wt });
                LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWt", MySqlDbType = MySqlDbType.Decimal, Value = Obj.Gross_Wt });

                LstParam.Add(new MySqlParameter { ParameterName = "in_CtStatus", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Ct_Status });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Destination", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Destination });
                LstParam.Add(new MySqlParameter { ParameterName = "in_SmtpNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Smtp_No });
                LstParam.Add(new MySqlParameter { ParameterName = "in_ReceivedDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Received_Date });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Genhaz", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Genhaz });
                LstParam.Add(new MySqlParameter { ParameterName = "in_ForeignLiner", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Foreign_Liner });
                LstParam.Add(new MySqlParameter { ParameterName = "in_VesselName", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Vessel_Name });
                LstParam.Add(new MySqlParameter { ParameterName = "in_VesselNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Vessel_No });
                LstParam.Add(new MySqlParameter { ParameterName = "in_TerminalLocation", MySqlDbType = MySqlDbType.VarChar, Value = "TKD" });
                LstParam.Add(new MySqlParameter { ParameterName = "In_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = Obj.PayeeId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = Obj.PayeeName });


                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainSummaryId", MySqlDbType = MySqlDbType.Int32, Value = Obj.TrainSummaryUploadId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.TrainNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainDate", MySqlDbType = MySqlDbType.DateTime, Value = Obj.TrainDate });
                LstParam.Add(new MySqlParameter { ParameterName = "In_PortId", MySqlDbType = MySqlDbType.Int32, Value = Obj.PortId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });
                LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "" });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("AddEditTrnSmaryUpldTKDV2", CommandType.StoredProcedure, DParam, out GeneratedClientId);
                _DBResponse = new DatabaseResponse();

                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Train summary and Invoice saved successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2 || Result == 3)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = GeneratedClientId;
                    _DBResponse.Data = "";
                }
                else
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = "";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        public void CheckTrainSummaryUploadTKD(string TrainNo, string TrainSummaryUploadXML, string TrainDate)
        {
            DataSet Result = new DataSet();
            try
            {
                //TrainDate = DateTime.ParseExact(TrainDate,"dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                int RetValue = 0;
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = TrainNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainDate", MySqlDbType = MySqlDbType.VarChar, Value = null });
                LstParam.Add(new MySqlParameter { ParameterName = "TrainSummaryUploadXML", MySqlDbType = MySqlDbType.Text, Value = TrainSummaryUploadXML });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = RetValue });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("checkTrnSmaryUpldTKDV2", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                RetValue = Convert.ToInt32(DParam.Where(x => x.ParameterName == "RetValue").Select(x => x.Value).FirstOrDefault());

                List<Ppg_TrainSummaryUploadV2> TrainSummaryUploadList = new List<Ppg_TrainSummaryUploadV2>();
                foreach (DataRow dr in Result.Tables[0].Rows)
                {

                    Ppg_TrainSummaryUploadV2 objTrainSummaryUpload = new Ppg_TrainSummaryUploadV2();
                    objTrainSummaryUpload.SrNo = TrainSummaryUploadList.Count + 1;
                    objTrainSummaryUpload.Wagon_No = Convert.ToString(dr["Wagon_No"]);
                    objTrainSummaryUpload.Container_No = Convert.ToString(dr["Container_No"]);
                    objTrainSummaryUpload.CT_Size = Convert.ToString(dr["CT_Size"]);
                    objTrainSummaryUpload.Line_Seal_No = Convert.ToString(dr["Line_Seal_No"]);
                    objTrainSummaryUpload.Cont_Commodity = Convert.ToString(dr["Cont_Commodity"]);
                    objTrainSummaryUpload.S_Line = Convert.ToString(dr["S_Line"]);
                    objTrainSummaryUpload.Foreign_Liner = Convert.ToString(dr["Foreign_Liner"]);
                    objTrainSummaryUpload.Vessel_Name = Convert.ToString(dr["Vessel_Name"]);
                    objTrainSummaryUpload.Vessel_No = Convert.ToString(dr["Vessel_No"]);
                    objTrainSummaryUpload.Ct_Tare = Convert.ToDecimal(dr["Ct_Tare"]);
                    objTrainSummaryUpload.Cargo_Wt = Convert.ToDecimal(dr["Cargo_Wt"]);
                    objTrainSummaryUpload.Gross_Wt = Convert.ToDecimal(dr["Gross_Wt"]);
                    objTrainSummaryUpload.Ct_Status = Convert.ToString(dr["Ct_Status"]);
                    objTrainSummaryUpload.Destination = Convert.ToString(dr["Destination"]);
                    objTrainSummaryUpload.Smtp_No = Convert.ToString(dr["Smtp_No"]);
                    objTrainSummaryUpload.Received_Date = Convert.ToString(dr["Received_Date"]);
                    objTrainSummaryUpload.Genhaz = Convert.ToString(dr["Genhaz"]);
                    objTrainSummaryUpload.Status = Convert.ToInt32(dr["Status"]);
                    objTrainSummaryUpload.PayeeId = Convert.ToInt32(dr["ShippingLineId"]);
                    objTrainSummaryUpload.PayeeName = Convert.ToString(dr["ShippingLineName"]);
                    /*if (objTrainSummaryUpload.Status == 0)
                    {
                        objTrainSummaryUpload.StatusDesc = "OK";
                    }
                    else if (objTrainSummaryUpload.Status == 1)
                    {
                        objTrainSummaryUpload.StatusDesc = "Already Exist.";
                    }
                    else if (objTrainSummaryUpload.Status == 2)
                    {
                        objTrainSummaryUpload.StatusDesc = "Cannot Save";
                    }*/


                    TrainSummaryUploadList.Add(objTrainSummaryUpload);
                }

                _DBResponse.Status = RetValue;
                _DBResponse.Message = "";
                _DBResponse.Data = TrainSummaryUploadList;
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

        public void GetTrainSummaryDetailsTKD(int TrainSummaryUploadId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainSummaryId", MySqlDbType = MySqlDbType.Int32, Value = TrainSummaryUploadId });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetTrainSummaryDetailsTKDV2", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                List<Ppg_TrainSummaryUploadV2> TrainSummaryUploadList = new List<Ppg_TrainSummaryUploadV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Ppg_TrainSummaryUploadV2 objTrainSummaryUpload = new Ppg_TrainSummaryUploadV2();
                        objTrainSummaryUpload.SrNo = TrainSummaryUploadList.Count + 1;
                        // objTrainSummaryUpload.Wagon_No = Convert.ToString(dr["Wagon_No"]);
                        objTrainSummaryUpload.Container_No = Convert.ToString(dr["Container_No"]);
                        objTrainSummaryUpload.CT_Size = Convert.ToString(dr["CT_Size"]);
                        objTrainSummaryUpload.Line_Seal_No = Convert.ToString(dr["Line_Seal_No"]);
                        objTrainSummaryUpload.Cont_Commodity = Convert.ToString(dr["Cont_Commodity"]);
                        objTrainSummaryUpload.S_Line = Convert.ToString(dr["S_Line"]);
                        //objTrainSummaryUpload.Ct_Tare = Convert.ToDecimal(dr["Ct_Tare"]);
                        //objTrainSummaryUpload.Cargo_Wt = Convert.ToDecimal(dr["Cargo_Wt"]);
                        //objTrainSummaryUpload.Gross_Wt = Convert.ToDecimal(dr["Gross_Wt"]);
                        objTrainSummaryUpload.Ct_Status = Convert.ToString(dr["Ct_Status"]);
                        //objTrainSummaryUpload.Destination = Convert.ToString(dr["Destination"]);
                        //objTrainSummaryUpload.Smtp_No = Convert.ToString(dr["Smtp_No"]);
                        //objTrainSummaryUpload.Received_Date = Convert.ToString(dr["Received_Date"]);
                        //objTrainSummaryUpload.Genhaz = Convert.ToString(dr["Genhaz"]);

                        objTrainSummaryUpload.PayeeName = Convert.ToString(dr["PayeeName"]);
                        objTrainSummaryUpload.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                        objTrainSummaryUpload.InvoiceAmt = Convert.ToDecimal(dr["InvoiceAmt"]);
                        objTrainSummaryUpload.Genhaz = Convert.ToString(dr["Genhaz"]);

                        objTrainSummaryUpload.StatusDesc = "";
                        TrainSummaryUploadList.Add(objTrainSummaryUpload);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = TrainSummaryUploadList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void ListOfTrainSummaryTKD()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetTrainSummaryListTKDV2", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<Ppg_TrainSummaryUploadV2> TrainSummaryUploadList = new List<Ppg_TrainSummaryUploadV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Ppg_TrainSummaryUploadV2 objTrainSummaryUpload = new Ppg_TrainSummaryUploadV2();
                        objTrainSummaryUpload.TrainSummaryUploadId = Convert.ToInt32(dr["TrainSummaryId"]);
                        objTrainSummaryUpload.TrainNo = Convert.ToString(dr["TrainNo"]);
                        objTrainSummaryUpload.TrainDate = Convert.ToString(dr["TrainDate"]);
                        objTrainSummaryUpload.PortId = Convert.ToInt32(dr["PortId"]);
                        objTrainSummaryUpload.PortName = Convert.ToString(dr["PortName"]);
                        objTrainSummaryUpload.UploadDate = Convert.ToString(dr["UploadDate"].ToString());

                        TrainSummaryUploadList.Add(objTrainSummaryUpload);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = TrainSummaryUploadList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void TrainSummryPayeeList(int Page, string PartyCode = "")
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Value = PartyCode });
                IDataParameter[] DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("TrainSummryPayeeListTKDV2", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<dynamic> lstShiping = new List<dynamic>();
                bool State = false;
                Status = 1;
                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        lstShiping.Add(new { EximTraderId = Convert.ToInt32(dr["EximTraderId"]), EximTraderName = dr["EximTraderName"].ToString(), EximTraderAlias = dr["EximTraderAlias"].ToString() });
                    }
                    if (Result.Tables[1].Rows.Count > 0)
                    {
                        State = Convert.ToBoolean(Result.Tables[1].Rows[0][0]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstShiping, State };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void BulkInvoiceDetailsofTrainForPrint(string TrainNo,int TrainSummaryUploadId)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TrainNo", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = TrainNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TrainSummaryUploadId", MySqlDbType = MySqlDbType.Int32, Value = TrainSummaryUploadId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("getbulkinvoicefortrnsummaryV2", CommandType.StoredProcedure, DParam);
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



        #region OBL Entry FCL
        public void ListOfShippingLinePartyCode(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForJO", CommandType.StoredProcedure, Dparam);
            IList<ShippingLineForPageV2> lstShippingLine = new List<ShippingLineForPageV2>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new ShippingLineForPageV2
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
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
                    _DBResponse.Data = new { lstShippingLine, State };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void ListOfImporterForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfImporterForPage", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.ImporterForPageV2> lstImporter = new List<Areas.Import.Models.ImporterForPageV2>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstImporter.Add(new Areas.Import.Models.ImporterForPageV2
                    {
                        ImporterId = Convert.ToInt32(Result["ImporterId"]),
                        ImporterName = Result["ImporterName"].ToString(),
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
        public void GetAllCommodityForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCommodityForPage", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.CommodityForPageV2> LstCommodity = new List<Areas.Import.Models.CommodityForPageV2>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstCommodity.Add(new Areas.Import.Models.CommodityForPageV2
                    {
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        CommodityName = Result["CommodityName"].ToString(),
                        PartyCode = Result["CommodityAlias"].ToString(),
                        CommodityType = Result["CommodityType"].ToString()
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
                    _DBResponse.Data = new { LstCommodity, State };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetOBLWiseContainerDetailsByID(int impobldtlId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_impobldtlId", MySqlDbType = MySqlDbType.Int32, Value = impobldtlId });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetEximAppOBLWiseContainerDetailsByID", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                OBLWiseContainerEntryV2 objOBLEntry = new OBLWiseContainerEntryV2();

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
                        objOBLEntry.CountryId = Convert.ToInt32(Result.Tables[0].Rows[0]["CountryId"]);
                        objOBLEntry.LineNo = Convert.ToString(Result.Tables[0].Rows[0]["LINE_NO"].ToString());
                        objOBLEntry.CargoDescription = Convert.ToString(Result.Tables[0].Rows[0]["CargoDescription"]);
                        objOBLEntry.NoOfPkg = Convert.ToString(Result.Tables[0].Rows[0]["NO_PKG"]);
                        objOBLEntry.PkgType = Convert.ToString(Result.Tables[0].Rows[0]["PKG_TYPE"]);
                        objOBLEntry.GR_WT = Convert.ToDecimal(Result.Tables[0].Rows[0]["GR_WT"]);
                        objOBLEntry.ImporterId = Convert.ToInt32(Result.Tables[0].Rows[0]["ImporterId"]);
                        objOBLEntry.ImporterName = Convert.ToString(Result.Tables[0].Rows[0]["ImporterName"]);
                        objOBLEntry.ICEGateImporterName = Convert.ToString(Result.Tables[0].Rows[0]["IGM_Imp_Name"]);
                        objOBLEntry.CargoType = Convert.ToInt32(Result.Tables[0].Rows[0]["CargoType"]);
                        objOBLEntry.SMTPNo = Convert.ToString(Result.Tables[0].Rows[0]["SMTPNo"]);
                        objOBLEntry.SMTP_Date = Convert.ToString(Result.Tables[0].Rows[0]["SMTP_Date"]);
                        objOBLEntry.CommodityId = Convert.ToInt32(Result.Tables[0].Rows[0]["CommodityId"]);
                        objOBLEntry.Commodity = Convert.ToString(Result.Tables[0].Rows[0]["Commodity"]);
                        objOBLEntry.IcesData = Convert.ToInt32(Result.Tables[0].Rows[0]["IcesData"]);
                    }

                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        OBLWiseContainerEntryDetailsV2 objOBLEntryDetails = new OBLWiseContainerEntryDetailsV2();
                        objOBLEntryDetails.ShippingLineName = Convert.ToString(dr["ShippingLineName"]);
                        objOBLEntryDetails.ShippingLineId = Convert.ToInt32(dr["ShippingLineId"]);
                        objOBLEntryDetails.ContainerSize = Convert.ToString(dr["ContainerSize"]);
                        objOBLEntryDetails.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objOBLEntryDetails.NoOfPkg = Convert.ToString(dr["NO_PKG"]);
                        objOBLEntryDetails.PkgType = Convert.ToString(dr["PKG_TYPE"]);
                        objOBLEntryDetails.GR_WT = Convert.ToDecimal(dr["GR_WT"]);
                        objOBLEntryDetails.impobldtlId = Convert.ToInt32(dr["impobldtlId"]);
                        objOBLEntryDetails.DetailsID = Convert.ToInt32(dr["DetailsID"]);
                        objOBLEntryDetails.ContIcesData = Convert.ToInt32(dr["ContIcesData"]);
                        objOBLEntry.OblEntryDetailsList.Add(objOBLEntryDetails);
                    }
                }

                if (Status == 1)
                {
                    if (objOBLEntry.OblEntryDetailsList.Count > 0)
                    {
                        objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(objOBLEntry.OblEntryDetailsList);
                    }

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

        public void GetOBLWiseContainerDetails(string OBLNo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLNo", MySqlDbType = MySqlDbType.String, Value = OBLNo });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLWiseContainerDetailsV2", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                OBLWiseContainerEntryV2 objOBLEntry = new OBLWiseContainerEntryV2();

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
                    }

                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        OBLWiseContainerEntryDetailsV2 objOBLEntryDetails = new OBLWiseContainerEntryDetailsV2();
                        objOBLEntryDetails.ShippingLineName = Convert.ToString(dr["ShippingLineName"]);
                        objOBLEntryDetails.ShippingLineId = Convert.ToInt32(dr["ShippingLineId"]);
                        objOBLEntryDetails.ContainerSize = Convert.ToString(dr["ContainerSize"]);
                        objOBLEntryDetails.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objOBLEntryDetails.NoOfPkg = Convert.ToString(dr["NO_PKG"]);
                        objOBLEntryDetails.GR_WT = Convert.ToDecimal(dr["GR_WT"]);
                        objOBLEntry.OblEntryDetailsList.Add(objOBLEntryDetails);
                    }
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
        public void AddEditOBLWiseContainerEntry(OBLWiseContainerEntryV2 objOBL, string XmlText)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.impobldtlId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLNO", MySqlDbType = MySqlDbType.String, Value = objOBL.OBL_No });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objOBL.OBL_Date) }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_LINE_NO", MySqlDbType = MySqlDbType.String, Value = objOBL.LineNo }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SMTPNo", MySqlDbType = MySqlDbType.String, Value = objOBL.SMTPNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SMTP_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objOBL.SMTP_Date) }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPkg", MySqlDbType = MySqlDbType.String, Value = objOBL.NoOfPkg });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = objOBL.CargoType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.String, Value = objOBL.CargoDescription });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PkgType", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.PkgType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GRWT", MySqlDbType = MySqlDbType.Decimal, Value = objOBL.GR_WT });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_No", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.IGM_No });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objOBL.IGM_Date).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TPNo", MySqlDbType = MySqlDbType.String, Value = objOBL.TPNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TPDate", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.TPDate != null ? Convert.ToDateTime(objOBL.TPDate).ToString("yyyy-MM-dd") : null });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MovementType", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.MovementType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.PortId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.CountryId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_Imp_Name", MySqlDbType = MySqlDbType.String, Value = objOBL.ICEGateImporterName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Ip", MySqlDbType = MySqlDbType.String, Value = objOBL.IP });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.ImporterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DtlXml", MySqlDbType = MySqlDbType.Text, Value = XmlText });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.CommodityId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IcesData", MySqlDbType = MySqlDbType.Int32, Value = objOBL.IcesData });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("EximAppAddEditOBLWiseContDet", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "OBL Entry FCL Saved Successfully" : "OBL Entry FCL Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "This OBL information is already exists!";
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
                    _DBResponse.Message = "Can not update as job order by train done!";
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

        public void ListOfOBLWiseContainer()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "in_uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("EximappListOfOBLWiseContainer", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<OBLWiseContainerEntryV2> OblEntryList = new List<OBLWiseContainerEntryV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        OBLWiseContainerEntryV2 objOBLEntry = new OBLWiseContainerEntryV2();
                        objOBLEntry.impobldtlId = Convert.ToInt32(dr["impobldtlId"]);
                        objOBLEntry.OBL_No = Convert.ToString(dr["OBL_No"]);
                        objOBLEntry.OBL_Date = Convert.ToString(dr["OBL_Date"]);
                        objOBLEntry.IGM_No = Convert.ToString(dr["IGM_No"].ToString());
                        objOBLEntry.IGM_Date = Convert.ToString(dr["IGM_Date"]);
                        //objOBLEntry.IsAlreadyUsed = Convert.ToInt32(dr["IsAlreadyUsed"]);
                        objOBLEntry.OBLCreateDate = Convert.ToString(dr["OBLCreate"]);
                        OblEntryList.Add(objOBLEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OblEntryList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
            int Result = DA.ExecuteNonQuery("EximappDeleteOBLWiseContainer", CommandType.StoredProcedure, DParam);
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
                    _DBResponse.Message = "Cannot Delete it is already approved";
                    _DBResponse.Status = 2;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Cannot Delete it is already Rejected";
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

        public void GetCheckOblEntryFCLApproval(int OblEntryId)
        {
            DataSet Result = new DataSet();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            Result = DA.ExecuteDataSet("EximAppOblEntryFCLApproved", CommandType.StoredProcedure, DParam);

            _DBResponse = new DatabaseResponse();
            try
            {

                _DBResponse.Data = Convert.ToInt32(Result.Tables[0].Rows[0]["Status"]);
                _DBResponse.Message = "Success";
                _DBResponse.Status = 1;



            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }
        public void GetListOfOBLWiseContainerSearchForApplication(string Obl)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();

                LstParam.Add(new MySqlParameter { ParameterName = "in_OBL", MySqlDbType = MySqlDbType.VarChar, Value = Obl });
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetListOfOBLWiseContainerSearchForApplicationV2", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<OBLWiseContainerEntryV2> OblEntryList = new List<OBLWiseContainerEntryV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        OBLWiseContainerEntryV2 objOBLEntry = new OBLWiseContainerEntryV2();
                        objOBLEntry.impobldtlId = Convert.ToInt32(dr["impobldtlId"]);
                        objOBLEntry.OBL_No = Convert.ToString(dr["OBL_No"]);
                        objOBLEntry.OBL_Date = Convert.ToString(dr["OBL_Date"]);
                        objOBLEntry.IGM_No = Convert.ToString(dr["IGM_No"].ToString());
                        objOBLEntry.IGM_Date = Convert.ToString(dr["IGM_Date"]);
                        //objOBLEntry.IsAlreadyUsed = Convert.ToInt32(dr["IsAlreadyUsed"]);
                        objOBLEntry.OBLCreateDate = Convert.ToString(dr["OBLCreate"]);
                        OblEntryList.Add(objOBLEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OblEntryList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region OBL Entry FCL Approval
        public void GetOBLEntryDetailsByOblNo()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "in_ImpOblDetailsID", MySqlDbType = MySqlDbType.Int32, Value = 0 });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("EximAppGetOblEntryDetailsFCL", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                List<OblList> objOBLEntry = new List<OblList>();

                //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        objOBLEntry.Add(new OblList
                        {
                            ID = Convert.ToInt32(dr["ImpOblDetailsID"]),
                            OBLNo = Convert.ToString(dr["OBL_No"])
                        });
                    }

                }

                if (Status == 1)
                {


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

        public void AddEditOBLWiseContainerEntryFCLApproval(OBLWiseContainerEntryV2 objOBL, string XmlText)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_impobldtlId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.OblDetailsId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.impobldtlId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLNO", MySqlDbType = MySqlDbType.String, Value = objOBL.OBL_No });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objOBL.OBL_Date) }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_LINE_NO", MySqlDbType = MySqlDbType.String, Value = objOBL.LineNo }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SMTPNo", MySqlDbType = MySqlDbType.String, Value = objOBL.SMTPNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SMTP_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objOBL.SMTP_Date) }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPkg", MySqlDbType = MySqlDbType.String, Value = objOBL.NoOfPkg });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = objOBL.CargoType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.String, Value = objOBL.CargoDescription });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PkgType", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.PkgType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GRWT", MySqlDbType = MySqlDbType.Decimal, Value = objOBL.GR_WT });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_No", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.IGM_No });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objOBL.IGM_Date).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TPNo", MySqlDbType = MySqlDbType.String, Value = objOBL.TPNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TPDate", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.TPDate != null ? Convert.ToDateTime(objOBL.TPDate).ToString("yyyy-MM-dd") : null });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MovementType", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.MovementType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.PortId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.CountryId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_Imp_Name", MySqlDbType = MySqlDbType.String, Value = objOBL.ICEGateImporterName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsApproveed", MySqlDbType = MySqlDbType.Int32, Value = objOBL.Approved });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Ip", MySqlDbType = MySqlDbType.String, Value = objOBL.IP });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.ImporterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DtlXml", MySqlDbType = MySqlDbType.Text, Value = XmlText });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.CommodityId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("EximAppAddEditOblEntryFClApproval", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "OBL Entry FCL Approved Successfully" : "OBL Entry FCL Rejected Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "OBL Entry FCL Approved Update Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "This OBL information is already exists!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as Custom Appraisement Application done!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 6)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as job order by train done!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 7)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Shipping line and container size should be same as gate entry";
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


        public void GetOBLWiseContainerDetailsApprovalByID(int impobldtlId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_impobldtlId", MySqlDbType = MySqlDbType.Int32, Value = impobldtlId });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetEximAppOBLWiseContainerDetailsApprovalByID", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                OBLWiseContainerEntryV2 objOBLEntry = new OBLWiseContainerEntryV2();

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
                        objOBLEntry.CountryId = Convert.ToInt32(Result.Tables[0].Rows[0]["CountryId"]);
                        objOBLEntry.LineNo = Convert.ToString(Result.Tables[0].Rows[0]["LINE_NO"].ToString());
                        objOBLEntry.CargoDescription = Convert.ToString(Result.Tables[0].Rows[0]["CargoDescription"]);
                        objOBLEntry.NoOfPkg = Convert.ToString(Result.Tables[0].Rows[0]["NO_PKG"]);
                        objOBLEntry.PkgType = Convert.ToString(Result.Tables[0].Rows[0]["PKG_TYPE"]);
                        objOBLEntry.GR_WT = Convert.ToDecimal(Result.Tables[0].Rows[0]["GR_WT"]);
                        objOBLEntry.ImporterId = Convert.ToInt32(Result.Tables[0].Rows[0]["ImporterId"]);
                        objOBLEntry.ImporterName = Convert.ToString(Result.Tables[0].Rows[0]["ImporterName"]);
                        objOBLEntry.ICEGateImporterName = Convert.ToString(Result.Tables[0].Rows[0]["IGM_Imp_Name"]);
                        objOBLEntry.CargoType = Convert.ToInt32(Result.Tables[0].Rows[0]["CargoType"]);
                        objOBLEntry.SMTPNo = Convert.ToString(Result.Tables[0].Rows[0]["SMTPNo"]);
                        objOBLEntry.SMTP_Date = Convert.ToString(Result.Tables[0].Rows[0]["SMTP_Date"]);
                        objOBLEntry.CommodityId = Convert.ToInt32(Result.Tables[0].Rows[0]["CommodityId"]);
                        objOBLEntry.Commodity = Convert.ToString(Result.Tables[0].Rows[0]["Commodity"]);
                    }

                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        OBLWiseContainerEntryDetailsV2 objOBLEntryDetails = new OBLWiseContainerEntryDetailsV2();
                        objOBLEntryDetails.ShippingLineName = Convert.ToString(dr["ShippingLineName"]);
                        objOBLEntryDetails.ShippingLineId = Convert.ToInt32(dr["ShippingLineId"]);
                        objOBLEntryDetails.ContainerSize = Convert.ToString(dr["ContainerSize"]);
                        objOBLEntryDetails.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objOBLEntryDetails.NoOfPkg = Convert.ToString(dr["NO_PKG"]);
                        objOBLEntryDetails.PkgType = Convert.ToString(dr["PKG_TYPE"]);
                        objOBLEntryDetails.GR_WT = Convert.ToDecimal(dr["GR_WT"]);
                        objOBLEntryDetails.impobldtlId = Convert.ToInt32(dr["impobldtlId"]);
                        objOBLEntryDetails.DetailsID = Convert.ToInt32(dr["DetailsID"]);
                        objOBLEntry.OblEntryDetailsList.Add(objOBLEntryDetails);
                    }
                }

                if (Status == 1)
                {
                    if (objOBLEntry.OblEntryDetailsList.Count > 0)
                    {
                        objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(objOBLEntry.OblEntryDetailsList);
                    }

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
        public void ListOfOBLWiseContainerApproval()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "in_uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("EximappListOfOBLWiseContainerApproval", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<OBLWiseContainerEntryV2> OblEntryList = new List<OBLWiseContainerEntryV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        OBLWiseContainerEntryV2 objOBLEntry = new OBLWiseContainerEntryV2();
                        objOBLEntry.impobldtlId = Convert.ToInt32(dr["impobldtlId"]);
                        objOBLEntry.OBL_No = Convert.ToString(dr["OBL_No"]);
                        objOBLEntry.OBL_Date = Convert.ToString(dr["OBL_Date"]);
                        objOBLEntry.IGM_No = Convert.ToString(dr["IGM_No"].ToString());
                        objOBLEntry.IGM_Date = Convert.ToString(dr["IGM_Date"]);
                        //objOBLEntry.IsAlreadyUsed = Convert.ToInt32(dr["IsAlreadyUsed"]);
                        objOBLEntry.OBLCreateDate = Convert.ToString(dr["OBLCreate"]);
                        OblEntryList.Add(objOBLEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OblEntryList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void DeleteOBLWiseContainerApproval(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("DeleteOBLWiseContainerApproval", CommandType.StoredProcedure, DParam);
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
                    _DBResponse.Message = "Cannot Delete As It Exists In Custom Appraisement Application";
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

        public void GetListOfOBLWiseContainerSearch(string Obl)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();

                LstParam.Add(new MySqlParameter { ParameterName = "in_OBL", MySqlDbType = MySqlDbType.VarChar, Value = Obl });
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetListOfOBLWiseContainerSearchV2", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<OBLWiseContainerEntryV2> OblEntryList = new List<OBLWiseContainerEntryV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        OBLWiseContainerEntryV2 objOBLEntry = new OBLWiseContainerEntryV2();
                        objOBLEntry.impobldtlId = Convert.ToInt32(dr["impobldtlId"]);
                        objOBLEntry.OBL_No = Convert.ToString(dr["OBL_No"]);
                        objOBLEntry.OBL_Date = Convert.ToString(dr["OBL_Date"]);
                        objOBLEntry.IGM_No = Convert.ToString(dr["IGM_No"].ToString());
                        objOBLEntry.IGM_Date = Convert.ToString(dr["IGM_Date"]);
                        //objOBLEntry.IsAlreadyUsed = Convert.ToInt32(dr["IsAlreadyUsed"]);
                        objOBLEntry.OBLCreateDate = Convert.ToString(dr["OBLCreate"]);
                        OblEntryList.Add(objOBLEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OblEntryList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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


        #region Obl Entry LCL
        public void GetOblEntryDetailsByOblEntryId(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetEximOblEntryDetailsByOblEntryId", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            OBLEntryV2 objOBLEntry = new OBLEntryV2();
            IList<OblEntryDetailsV2> lstOBLEntryDetails = new List<OblEntryDetailsV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objOBLEntry.Id = Convert.ToInt32(Result["Id"]);
                    objOBLEntry.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    objOBLEntry.ContainerSize = Convert.ToString(Result["ContainerSize"]);
                    objOBLEntry.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objOBLEntry.IGM_No = Convert.ToString(Result["IGM_No"]);
                    objOBLEntry.IGM_Date = Convert.ToString(Result["IGM_Date"]);
                    objOBLEntry.TPNo = Convert.ToString(Result["TPNo"]);
                    objOBLEntry.TPDate = Convert.ToString(Result["TPDate"]);
                    objOBLEntry.MovementType = Convert.ToString(Result["MovementType"]);
                    objOBLEntry.PortId = Convert.ToInt32(Result["PortId"].ToString());
                    objOBLEntry.PortName = Convert.ToString(Result["PortName"]);
                    objOBLEntry.CountryId = Convert.ToInt32(Result["CountryId"].ToString());
                    objOBLEntry.CountryName = Convert.ToString(Result["CountryName"]);
                    objOBLEntry.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    objOBLEntry.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objOBLEntry.IsAlreadyUsed = Convert.ToInt32(Result["IsAlreadyUsed"]);
                    objOBLEntry.CONTCBT = Convert.ToString(Result["CONTCBT"]);
                    objOBLEntry.ContIcesData = Convert.ToInt32(Result["ContIcesData"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstOBLEntryDetails.Add(new OblEntryDetailsV2
                        {
                            OBLEntryId = Convert.ToInt32(Result["OblEntry_DtlId"]),
                            OBL_No = Convert.ToString(Result["OBL_NO"]),
                            OBL_Date = Convert.ToString(Result["OBL_DATE"]),
                            SMTP_Date = Convert.ToString(Result["SMTP_Date"]),
                            LineNo = Convert.ToString(Result["LINE_NO"].ToString()),
                            CargoDescription = Convert.ToString(Result["CargoDescription"]),
                            NoOfPkg = Convert.ToString(Result["NO_PKG"]),
                            PkgType = Convert.ToString(Result["PKG_TYPE"]),
                            GR_WT = Convert.ToDecimal(Result["GR_WT"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"]),
                            ImporterName = Convert.ToString(Result["ImporterName"]),
                            SMTPNo = Convert.ToString(Result["SMTPNo"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            IsProcessed = Convert.ToInt32(Result["IsProcessed"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            Commodity = Convert.ToString(Result["Commodity"]),
                            IGM_IMPORTER = Convert.ToString(Result["IGM_Importer"]),
                            IcesData = Convert.ToInt32(Result["IcesData"])
                    });
                    }
                }
                if (lstOBLEntryDetails.Count > 0)
                {
                    objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(lstOBLEntryDetails);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objOBLEntry;
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
        public void GetOBLContainerListOrSize(string CFSCode = "")
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.String, Value = CFSCode });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getoblcontainerlistV2", CommandType.StoredProcedure, DParam);
            List<ContainerInfoV2> LstContainerInfo = new List<ContainerInfoV2>();
            string size = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    if (CFSCode == "" || CFSCode == null)
                    {
                        LstContainerInfo.Add(new ContainerInfoV2
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            MovementType = Convert.ToString(Result["MovementType"]),
                            EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                            EximTraderName = Convert.ToString(Result["EximTraderName"])
                        });
                    }
                    else
                    {
                        size = Convert.ToString(Result["Size"]);
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    if (CFSCode == "" || CFSCode == null)
                    {
                        _DBResponse.Data = LstContainerInfo;
                    }
                    else
                    {
                        _DBResponse.Data = size;
                    }

                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetCFSCodeFromContainer(string ContainerNo, string ContainerSize, string CFSCode)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_CONTAINER_NO", MySqlDbType = MySqlDbType.String, Value = ContainerNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CONT_SIZE", MySqlDbType = MySqlDbType.String, Value = ContainerSize });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CFSCode", MySqlDbType = MySqlDbType.String, Value = CFSCode });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetCFSCodeFromContainer", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                int RetVal = 0;
                //string CFSCode = "";

                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        RetVal = Convert.ToInt32(Result.Tables[0].Rows[0]["RetVal"]);
                        CFSCode = Convert.ToString(Result.Tables[0].Rows[0]["CFSCode"]);
                    }

                }

                if (Status == 1)
                {
                    _DBResponse.Status = RetVal;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CFSCode;
                }
                else
                {
                    _DBResponse.Status = -1;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }
        public void GetOBLDetails(string CFSCode, string ContainerNo, string ContainerSize, string IGM_No, string IGM_Date, int OBLEntryId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_CFSCode", MySqlDbType = MySqlDbType.String, Value = CFSCode });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CONTAINER_NO", MySqlDbType = MySqlDbType.String, Value = ContainerNo });
                //LstParam.Add(new MySqlParameter { ParameterName = "In_CONT_SIZE", MySqlDbType = MySqlDbType.String, Value = ContainerSize });
                //LstParam.Add(new MySqlParameter { ParameterName = "In_IGM_NO", MySqlDbType = MySqlDbType.String, Value = IGM_No });
                //LstParam.Add(new MySqlParameter { ParameterName = "In_IGM_DATE", MySqlDbType = MySqlDbType.DateTime, Value = dt.ToString("yyyy/MM/dd") });
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLEntryId", MySqlDbType = MySqlDbType.Int32, Value = OBLEntryId });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOblEntryDetails", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                OBLEntryV2 objOBLEntry = new OBLEntryV2();

                //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        objOBLEntry.MovementType = Convert.ToString(Result.Tables[0].Rows[0]["MovementType"]);
                        objOBLEntry.PortId = Convert.ToInt32(Result.Tables[0].Rows[0]["PortId"]);
                        objOBLEntry.ShippingLineName = Convert.ToString(Result.Tables[0].Rows[0]["ShippingLineName"]);
                        objOBLEntry.ShippingLineId = Convert.ToInt32(Result.Tables[0].Rows[0]["ShippingLineId"]);
                        objOBLEntry.ContainerSize = Convert.ToString(Result.Tables[0].Rows[0]["ContainerSize"]);
                        objOBLEntry.IGM_No = Convert.ToString(Result.Tables[0].Rows[0]["IGM_NO"]);
                        objOBLEntry.IGM_Date = Convert.ToString(Result.Tables[0].Rows[0]["IGM_DATE"]);
                        objOBLEntry.TPNo = Convert.ToString(Result.Tables[0].Rows[0]["TP_NO"]);
                        objOBLEntry.TPDate = Convert.ToString(Result.Tables[0].Rows[0]["TP_DATE"]);
                    }

                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        OblEntryDetailsV2 objOBLEntryDetails = new OblEntryDetailsV2();
                        objOBLEntryDetails.icesContId = Convert.ToInt32(dr["icesContId"]);
                        objOBLEntryDetails.OBL_No = Convert.ToString(dr["OBL_NO"]);
                        objOBLEntryDetails.OBL_Date = Convert.ToString(dr["OBL_DATE"]);
                        objOBLEntryDetails.LineNo = Convert.ToString(dr["LINE_NO"].ToString());
                        objOBLEntryDetails.CargoDescription = Convert.ToString(dr["CargoDescription"]);
                        objOBLEntryDetails.NoOfPkg = Convert.ToString(dr["NO_PKG"]);
                        objOBLEntryDetails.PkgType = Convert.ToString(dr["PKG_TYPE"]);
                        objOBLEntryDetails.GR_WT = Convert.ToDecimal(dr["GR_WT"]);
                        objOBLEntryDetails.ImporterId = Convert.ToInt32(dr["ImporterId"]);
                        objOBLEntryDetails.ImporterName = Convert.ToString(dr["ImporterName"]);
                        objOBLEntryDetails.CargoType = Convert.ToInt32(dr["CargoType"]);
                        objOBLEntryDetails.SMTPNo = Convert.ToString(dr["SMTPNo"]);
                        objOBLEntryDetails.IsProcessed = Convert.ToInt32(dr["IsProcessed"]);
                        objOBLEntryDetails.IGM_IMPORTER = Convert.ToString(dr["IGM_IMPORTER"]);
                        objOBLEntry.OblEntryDetailsList.Add(objOBLEntryDetails);
                    }
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
        public void GetAllOblEntry(int Page)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
                LstParam.Add(new MySqlParameter { ParameterName = "in_uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });

                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllEximAppOblEntryForPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<OBLEntryV2> OblEntryList = new List<OBLEntryV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        OBLEntryV2 objOBLEntry = new OBLEntryV2();
                        objOBLEntry.Id = Convert.ToInt32(dr["Id"]);
                        objOBLEntry.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objOBLEntry.ContainerSize = Convert.ToString(dr["ContainerSize"]);
                        objOBLEntry.IGM_No = Convert.ToString(dr["IGM_No"].ToString());
                        objOBLEntry.IGM_Date = Convert.ToString(dr["IGM_Date"]);
                        objOBLEntry.IsAlreadyUsed = Convert.ToInt32(dr["IsAlreadyUsed"]);
                        objOBLEntry.OBLCreateDate = Convert.ToString(dr["OBLCreateDate"]);
                        OblEntryList.Add(objOBLEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OblEntryList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void DeleteOBLEntry(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("EximAppDeleteOBLEntry", CommandType.StoredProcedure, DParam);
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
                    _DBResponse.Message = "Can not Delete is already Approval!";
                    _DBResponse.Status = 2;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Cannot Delete  As It Exists In Job Order By Road";
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
        public void AddEditOBLEntry(OBLEntryV2 objOBL, string XmlText)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.String, Value = objOBL.ContainerNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CONTCBT", MySqlDbType = MySqlDbType.String, Value = objOBL.CONTCBT });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.String, Value = objOBL.ContainerSize }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.String, Value = objOBL.CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_No", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.IGM_No });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objOBL.IGM_Date).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TPNo", MySqlDbType = MySqlDbType.String, Value = objOBL.TPNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TPDate", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.TPDate != null ? Convert.ToDateTime(objOBL.TPDate).ToString("yyyy-MM-dd") : null });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MovementType", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.MovementType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.PortId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.CountryId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.ShippingLineId });

            lstParam.Add(new MySqlParameter { ParameterName = "in_DtlXml", MySqlDbType = MySqlDbType.Text, Value = XmlText });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Ip", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.IP });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContIcesData", MySqlDbType = MySqlDbType.Int32, Value = objOBL.ContIcesData });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("EximAppAddEditOBLEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
                    _DBResponse.Message = "Can not update is already Approval!";
                    _DBResponse.Status = Result;
                }

                else if (Result == 8)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as TallySheet done!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as job order by road done!";
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

        public void GetListOfOBLEntryByContainerNoForApplication(string ContainerNo)
        {
            int Status = 0;
            string[] EmailSplit = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfOBLEntryByContainerNoApplicationV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OBLEntryV2> OblEntryList = new List<OBLEntryV2>();
            ///  List<PPGEximTrader> LstEximTrader = new List<PPGEximTrader>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // EmailSplit = ((Result["Email"] == null ? "" : Result["Email"]).ToString()).Trim().Split(',');
                    // var Email = string.Join("\n ", EmailSplit);
                    OblEntryList.Add(new OBLEntryV2
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ContainerSize = Convert.ToString(Result["ContainerSize"]),
                        IGM_No = Convert.ToString(Result["IGM_No"].ToString()),
                        IGM_Date = Convert.ToString(Result["IGM_Date"]),
                        IsAlreadyUsed = Convert.ToInt32(Result["IsAlreadyUsed"]),
                        OBLCreateDate = Convert.ToString(Result["OBLCreateDate"])

                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OblEntryList;
                }
                else
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


        #region OBL Entry LCL Approval
        public void GetOblEntryDetailsByOblEntryApprovalId(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetEximOblEntryDetailsByOblEntryId", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            OBLEntryV2 objOBLEntry = new OBLEntryV2();
            IList<OblEntryDetailsV2> lstOBLEntryDetails = new List<OblEntryDetailsV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objOBLEntry.EximHrdId = Convert.ToInt32(Result["Id"]);
                    objOBLEntry.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    objOBLEntry.ContainerSize = Convert.ToString(Result["ContainerSize"]);
                    objOBLEntry.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objOBLEntry.IGM_No = Convert.ToString(Result["IGM_No"]);
                    objOBLEntry.IGM_Date = Convert.ToString(Result["IGM_Date"]);
                    objOBLEntry.TPNo = Convert.ToString(Result["TPNo"]);
                    objOBLEntry.TPDate = Convert.ToString(Result["TPDate"]);
                    objOBLEntry.MovementType = Convert.ToString(Result["MovementType"]);
                    objOBLEntry.PortId = Convert.ToInt32(Result["PortId"].ToString());
                    objOBLEntry.PortName = Convert.ToString(Result["PortName"]);
                    objOBLEntry.CountryId = Convert.ToInt32(Result["CountryId"].ToString());
                    objOBLEntry.CountryName = Convert.ToString(Result["CountryName"]);
                    objOBLEntry.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    objOBLEntry.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objOBLEntry.IsAlreadyUsed = Convert.ToInt32(Result["IsAlreadyUsed"]);
                    objOBLEntry.CONTCBT = Convert.ToString(Result["CONTCBT"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstOBLEntryDetails.Add(new OblEntryDetailsV2
                        {
                            OBLEntryId = Convert.ToInt32(Result["OblEntry_DtlId"]),
                            OBL_No = Convert.ToString(Result["OBL_NO"]),
                            OBL_Date = Convert.ToString(Result["OBL_DATE"]),
                            SMTP_Date = Convert.ToString(Result["SMTP_Date"]),
                            LineNo = Convert.ToString(Result["LINE_NO"].ToString()),
                            CargoDescription = Convert.ToString(Result["CargoDescription"]),
                            NoOfPkg = Convert.ToString(Result["NO_PKG"]),
                            PkgType = Convert.ToString(Result["PKG_TYPE"]),
                            GR_WT = Convert.ToDecimal(Result["GR_WT"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"]),
                            ImporterName = Convert.ToString(Result["ImporterName"]),
                            SMTPNo = Convert.ToString(Result["SMTPNo"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            IsProcessed = Convert.ToInt32(Result["IsProcessed"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            Commodity = Convert.ToString(Result["Commodity"]),
                            IGM_IMPORTER = Convert.ToString(Result["IGM_Importer"])
                        });
                    }
                }
                if (lstOBLEntryDetails.Count > 0)
                {
                    objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(lstOBLEntryDetails);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objOBLEntry;
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
        public void GetOBLContainerListOrSizeApproval(string CFSCode = "")
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.String, Value = CFSCode });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getEximAppoblcontainerlistApproval", CommandType.StoredProcedure, DParam);
            List<ContainerInfo> LstContainerInfo = new List<ContainerInfo>();
            string size = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    if (CFSCode == "" || CFSCode == null)
                    {
                        LstContainerInfo.Add(new ContainerInfo
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            MovementType = Convert.ToString(Result["MovementType"]),
                            OblhrdID = Convert.ToInt32(Result["OblHrdID"]),

                        });
                    }
                    else
                    {
                        size = Convert.ToString(Result["Size"]);
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    if (CFSCode == "" || CFSCode == null)
                    {
                        _DBResponse.Data = LstContainerInfo;
                    }
                    else
                    {
                        _DBResponse.Data = size;
                    }

                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void AddEditOBLEntryApproval(OBLEntryV2 objOBL, string XmlText)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblHrdID", MySqlDbType = MySqlDbType.Int32, Value = objOBL.EximHrdId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsApproved", MySqlDbType = MySqlDbType.Int32, Value = objOBL.IsApproved });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.String, Value = objOBL.ContainerNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CONTCBT", MySqlDbType = MySqlDbType.String, Value = objOBL.CONTCBT });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.String, Value = objOBL.ContainerSize }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.String, Value = objOBL.CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_No", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.IGM_No });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objOBL.IGM_Date).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TPNo", MySqlDbType = MySqlDbType.String, Value = objOBL.TPNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TPDate", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.TPDate != null ? Convert.ToDateTime(objOBL.TPDate).ToString("yyyy-MM-dd") : null });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MovementType", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.MovementType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.PortId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.CountryId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.ShippingLineId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DtlXml", MySqlDbType = MySqlDbType.Text, Value = XmlText });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("EximAppAddEditOBLEntryApproval", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "OBL Entry Has Been Approved Successfully" : "OBL Entry Updated Successfully";
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
                    _DBResponse.Message = "Can not update as TallySheet done!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as job order by road done!";
                    _DBResponse.Status = Result;
                }
                else if (Result ==6)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "OBL Entry Has Been Rejected Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 7)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Shipping line and container size should be same as gate entry";
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

        public void GetAllOblEntryApproval(int Page)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllEximApprovalOblEntryForPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<OBLEntryV2> OblEntryList = new List<OBLEntryV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        OBLEntryV2 objOBLEntry = new OBLEntryV2();
                        objOBLEntry.Id = Convert.ToInt32(dr["Id"]);
                        objOBLEntry.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objOBLEntry.ContainerSize = Convert.ToString(dr["ContainerSize"]);
                        objOBLEntry.IGM_No = Convert.ToString(dr["IGM_No"].ToString());
                        objOBLEntry.IGM_Date = Convert.ToString(dr["IGM_Date"]);
                        objOBLEntry.IsAlreadyUsed = Convert.ToInt32(dr["IsAlreadyUsed"]);
                        objOBLEntry.OBLCreateDate = Convert.ToString(dr["OBLCreateDate"]);
                        OblEntryList.Add(objOBLEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OblEntryList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetOblEntryDetailsByOblEntryApprovalIdEdit(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetEximOblEntryApprovalDetailsByOblEntryId", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            OBLEntryV2 objOBLEntry = new OBLEntryV2();
            IList<OblEntryDetailsV2> lstOBLEntryDetails = new List<OblEntryDetailsV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objOBLEntry.Id = Convert.ToInt32(Result["Id"]);
                    objOBLEntry.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    objOBLEntry.ContainerSize = Convert.ToString(Result["ContainerSize"]);
                    objOBLEntry.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objOBLEntry.IGM_No = Convert.ToString(Result["IGM_No"]);
                    objOBLEntry.IGM_Date = Convert.ToString(Result["IGM_Date"]);
                    objOBLEntry.TPNo = Convert.ToString(Result["TPNo"]);
                    objOBLEntry.TPDate = Convert.ToString(Result["TPDate"]);
                    objOBLEntry.MovementType = Convert.ToString(Result["MovementType"]);
                    objOBLEntry.PortId = Convert.ToInt32(Result["PortId"].ToString());
                    objOBLEntry.PortName = Convert.ToString(Result["PortName"]);
                    objOBLEntry.CountryId = Convert.ToInt32(Result["CountryId"].ToString());
                    objOBLEntry.CountryName = Convert.ToString(Result["CountryName"]);
                    objOBLEntry.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    objOBLEntry.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objOBLEntry.IsAlreadyUsed = Convert.ToInt32(Result["IsAlreadyUsed"]);
                    objOBLEntry.CONTCBT = Convert.ToString(Result["CONTCBT"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstOBLEntryDetails.Add(new OblEntryDetailsV2
                        {
                            OBLEntryId = Convert.ToInt32(Result["OblEntry_DtlId"]),
                            OBL_No = Convert.ToString(Result["OBL_NO"]),
                            OBL_Date = Convert.ToString(Result["OBL_DATE"]),
                            SMTP_Date = Convert.ToString(Result["SMTP_Date"]),
                            LineNo = Convert.ToString(Result["LINE_NO"].ToString()),
                            CargoDescription = Convert.ToString(Result["CargoDescription"]),
                            NoOfPkg = Convert.ToString(Result["NO_PKG"]),
                            PkgType = Convert.ToString(Result["PKG_TYPE"]),
                            GR_WT = Convert.ToDecimal(Result["GR_WT"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"]),
                            ImporterName = Convert.ToString(Result["ImporterName"]),
                            SMTPNo = Convert.ToString(Result["SMTPNo"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            IsProcessed = Convert.ToInt32(Result["IsProcessed"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            Commodity = Convert.ToString(Result["Commodity"]),
                            IGM_IMPORTER = Convert.ToString(Result["IGM_Importer"])
                        });
                    }
                }
                if (lstOBLEntryDetails.Count > 0)
                {
                    objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(lstOBLEntryDetails);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objOBLEntry;
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


        public void DeleteOBLEntryApproval(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("EximappDeleteOBLEntryApproval", CommandType.StoredProcedure, DParam);
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
                    _DBResponse.Message = "Cannot Delete  As It Exists In Job Order By Road";
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
        public void GetListOfOBLEntryByContainerNo(string ContainerNo)
        {
            int Status = 0;
            string[] EmailSplit = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfOBLEntryByContainerNoV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OBLEntryV2> OblEntryList = new List<OBLEntryV2>();
            ///  List<PPGEximTrader> LstEximTrader = new List<PPGEximTrader>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // EmailSplit = ((Result["Email"] == null ? "" : Result["Email"]).ToString()).Trim().Split(',');
                    // var Email = string.Join("\n ", EmailSplit);
                    OblEntryList.Add(new OBLEntryV2
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ContainerSize = Convert.ToString(Result["ContainerSize"]),
                        IGM_No = Convert.ToString(Result["IGM_No"].ToString()),
                        IGM_Date = Convert.ToString(Result["IGM_Date"]),
                        IsAlreadyUsed = Convert.ToInt32(Result["IsAlreadyUsed"]),
                        OBLCreateDate = Convert.ToString(Result["OBLCreateDate"])

                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OblEntryList;
                }
                else
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

        #region Train Summary LONI
        public void AddUpdateTrainSummaryUploadLONI(Ppg_TrainSummaryUploadV2 Obj,string SEZ)
        {
            try
            {
                Obj.TrainDate = DateTime.ParseExact(Obj.TrainDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                string GeneratedClientId = "";
                List<MySqlParameter> LstParam = new List<MySqlParameter>();

                LstParam.Add(new MySqlParameter { ParameterName = "in_WagonNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Wagon_No });
                LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Container_No });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = Obj.CT_Size });
                LstParam.Add(new MySqlParameter { ParameterName = "in_LineSealNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Line_Seal_No });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Commodity", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Cont_Commodity });
                LstParam.Add(new MySqlParameter { ParameterName = "in_SLine", MySqlDbType = MySqlDbType.VarChar, Value = Obj.S_Line });

                LstParam.Add(new MySqlParameter { ParameterName = "in_CtTare", MySqlDbType = MySqlDbType.Decimal, Value = Obj.Ct_Tare });
                LstParam.Add(new MySqlParameter { ParameterName = "in_CargoWt", MySqlDbType = MySqlDbType.Decimal, Value = Obj.Cargo_Wt });
                LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWt", MySqlDbType = MySqlDbType.Decimal, Value = Obj.Gross_Wt });

                LstParam.Add(new MySqlParameter { ParameterName = "in_CtStatus", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Ct_Status });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Destination", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Destination });
                LstParam.Add(new MySqlParameter { ParameterName = "in_SmtpNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Smtp_No });
                LstParam.Add(new MySqlParameter { ParameterName = "in_ReceivedDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Received_Date });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Genhaz", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Genhaz });
                LstParam.Add(new MySqlParameter { ParameterName = "in_ForeignLiner", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Foreign_Liner });
                LstParam.Add(new MySqlParameter { ParameterName = "in_VesselName", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Vessel_Name });
                LstParam.Add(new MySqlParameter { ParameterName = "in_VesselNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Vessel_No });
                LstParam.Add(new MySqlParameter { ParameterName = "in_TerminalLocation", MySqlDbType = MySqlDbType.VarChar, Value = "ACTL" });
                LstParam.Add(new MySqlParameter { ParameterName = "In_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = Obj.PayeeId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = Obj.PayeeName });
                LstParam.Add(new MySqlParameter { ParameterName = "In_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });


                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainSummaryId", MySqlDbType = MySqlDbType.Int32, Value = Obj.TrainSummaryUploadId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.TrainNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainDate", MySqlDbType = MySqlDbType.DateTime, Value = Obj.TrainDate });
                LstParam.Add(new MySqlParameter { ParameterName = "In_PortId", MySqlDbType = MySqlDbType.Int32, Value = Obj.PortId });
                LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "" });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("AddEditTrnSmaryUpldLoniV2", CommandType.StoredProcedure, DParam, out GeneratedClientId);
                _DBResponse = new DatabaseResponse();

                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Train summary and Invoice saved successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2 || Result == 3)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = GeneratedClientId;
                    _DBResponse.Data = "";
                }
                else
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = "";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        public void CheckTrainSummaryUploadLONI(string TrainNo, string TrainSummaryUploadXML, string TrainDate)
        {
            DataSet Result = new DataSet();
            try
            {
                TrainDate = DateTime.ParseExact(TrainDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                int RetValue = 0;
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = TrainNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainDate", MySqlDbType = MySqlDbType.VarChar, Value = TrainDate });
                LstParam.Add(new MySqlParameter { ParameterName = "TrainSummaryUploadXML", MySqlDbType = MySqlDbType.Text, Value = TrainSummaryUploadXML });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = RetValue });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("checkTrnSmaryUpldLoniV2", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                RetValue = Convert.ToInt32(DParam.Where(x => x.ParameterName == "RetValue").Select(x => x.Value).FirstOrDefault());

                List<Ppg_TrainSummaryUploadV2> TrainSummaryUploadList = new List<Ppg_TrainSummaryUploadV2>();
                foreach (DataRow dr in Result.Tables[0].Rows)
                {

                    Ppg_TrainSummaryUploadV2 objTrainSummaryUpload = new Ppg_TrainSummaryUploadV2();
                    objTrainSummaryUpload.SrNo = TrainSummaryUploadList.Count + 1;
                    objTrainSummaryUpload.Wagon_No = Convert.ToString(dr["Wagon_No"]);
                    objTrainSummaryUpload.Container_No = Convert.ToString(dr["Container_No"]);
                    objTrainSummaryUpload.CT_Size = Convert.ToString(dr["CT_Size"]);
                    objTrainSummaryUpload.Line_Seal_No = Convert.ToString(dr["Line_Seal_No"]);
                    objTrainSummaryUpload.Cont_Commodity = Convert.ToString(dr["Cont_Commodity"]);
                    objTrainSummaryUpload.S_Line = Convert.ToString(dr["S_Line"]);
                    objTrainSummaryUpload.Foreign_Liner = Convert.ToString(dr["Foreign_Liner"]);
                    objTrainSummaryUpload.Vessel_Name = Convert.ToString(dr["Vessel_Name"]);
                    objTrainSummaryUpload.Vessel_No = Convert.ToString(dr["Vessel_No"]);
                    objTrainSummaryUpload.Ct_Tare = Convert.ToDecimal(dr["Ct_Tare"]);
                    objTrainSummaryUpload.Cargo_Wt = Convert.ToDecimal(dr["Cargo_Wt"]);
                    objTrainSummaryUpload.Gross_Wt = Convert.ToDecimal(dr["Gross_Wt"]);
                    objTrainSummaryUpload.Ct_Status = Convert.ToString(dr["Ct_Status"]);
                    objTrainSummaryUpload.Destination = Convert.ToString(dr["Destination"]);
                    objTrainSummaryUpload.Smtp_No = Convert.ToString(dr["Smtp_No"]);
                    objTrainSummaryUpload.Received_Date = Convert.ToString(dr["Received_Date"]);
                    objTrainSummaryUpload.Genhaz = Convert.ToString(dr["Genhaz"]);
                    objTrainSummaryUpload.Status = Convert.ToInt32(dr["Status"]);
                    objTrainSummaryUpload.PayeeId = Convert.ToInt32(dr["ShippingLineId"]);
                    objTrainSummaryUpload.PayeeName = Convert.ToString(dr["ShippingLineName"]);
                    /*if (objTrainSummaryUpload.Status == 0)
                    {
                        objTrainSummaryUpload.StatusDesc = "OK";
                    }
                    else if (objTrainSummaryUpload.Status == 1)
                    {
                        objTrainSummaryUpload.StatusDesc = "Already Exist.";
                    }
                    else if (objTrainSummaryUpload.Status == 2)
                    {
                        objTrainSummaryUpload.StatusDesc = "Cannot Save";
                    }*/


                    TrainSummaryUploadList.Add(objTrainSummaryUpload);
                }

                _DBResponse.Status = RetValue;
                _DBResponse.Message = "";
                _DBResponse.Data = TrainSummaryUploadList;
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
        public void GetTrainSummaryDetailsLONI(int TrainSummaryUploadId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainSummaryId", MySqlDbType = MySqlDbType.Int32, Value = TrainSummaryUploadId });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetTrainSummaryDetailsLONIV2", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                List<Ppg_TrainSummaryUploadV2> TrainSummaryUploadList = new List<Ppg_TrainSummaryUploadV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Ppg_TrainSummaryUploadV2 objTrainSummaryUpload = new Ppg_TrainSummaryUploadV2();
                        objTrainSummaryUpload.SrNo = TrainSummaryUploadList.Count + 1;
                        // objTrainSummaryUpload.Wagon_No = Convert.ToString(dr["Wagon_No"]);
                        objTrainSummaryUpload.Container_No = Convert.ToString(dr["Container_No"]);
                        objTrainSummaryUpload.CT_Size = Convert.ToString(dr["CT_Size"]);
                        objTrainSummaryUpload.Line_Seal_No = Convert.ToString(dr["Line_Seal_No"]);
                        objTrainSummaryUpload.Cont_Commodity = Convert.ToString(dr["Cont_Commodity"]);
                        objTrainSummaryUpload.S_Line = Convert.ToString(dr["S_Line"]);
                        //objTrainSummaryUpload.Ct_Tare = Convert.ToDecimal(dr["Ct_Tare"]);
                        //objTrainSummaryUpload.Cargo_Wt = Convert.ToDecimal(dr["Cargo_Wt"]);
                        //objTrainSummaryUpload.Gross_Wt = Convert.ToDecimal(dr["Gross_Wt"]);
                        objTrainSummaryUpload.Ct_Status = Convert.ToString(dr["Ct_Status"]);
                        //objTrainSummaryUpload.Destination = Convert.ToString(dr["Destination"]);
                        //objTrainSummaryUpload.Smtp_No = Convert.ToString(dr["Smtp_No"]);
                        //objTrainSummaryUpload.Received_Date = Convert.ToString(dr["Received_Date"]);
                        //objTrainSummaryUpload.Genhaz = Convert.ToString(dr["Genhaz"]);

                        objTrainSummaryUpload.PayeeName = Convert.ToString(dr["PayeeName"]);
                        objTrainSummaryUpload.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                        objTrainSummaryUpload.InvoiceAmt = Convert.ToDecimal(dr["InvoiceAmt"]);
                        objTrainSummaryUpload.Genhaz = Convert.ToString(dr["Genhaz"]);

                        objTrainSummaryUpload.StatusDesc = "";
                        TrainSummaryUploadList.Add(objTrainSummaryUpload);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = TrainSummaryUploadList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void ListOfTrainSummaryLONI()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetTrainSummaryListLoniV2", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<Ppg_TrainSummaryUploadV2> TrainSummaryUploadList = new List<Ppg_TrainSummaryUploadV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Ppg_TrainSummaryUploadV2 objTrainSummaryUpload = new Ppg_TrainSummaryUploadV2();
                        objTrainSummaryUpload.TrainSummaryUploadId = Convert.ToInt32(dr["TrainSummaryId"]);
                        objTrainSummaryUpload.TrainNo = Convert.ToString(dr["TrainNo"]);
                        objTrainSummaryUpload.TrainDate = Convert.ToString(dr["TrainDate"]);
                        objTrainSummaryUpload.PortId = Convert.ToInt32(dr["PortId"]);
                        objTrainSummaryUpload.PortName = Convert.ToString(dr["PortName"]);
                        objTrainSummaryUpload.UploadDate = Convert.ToString(dr["UploadDate"].ToString());

                        TrainSummaryUploadList.Add(objTrainSummaryUpload);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = TrainSummaryUploadList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void TrainSummryPayeeListLONI(int Page, string PartyCode)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Value = PartyCode });
                IDataParameter[] DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("TrainSummryPayeeListLONIV2", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<dynamic> lstShiping = new List<dynamic>();
                bool State = false;
                Status = 1;
                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        lstShiping.Add(new { EximTraderId = Convert.ToInt32(dr["EximTraderId"]), EximTraderName = dr["EximTraderName"].ToString(), EximTraderAlias = dr["EximTraderAlias"].ToString() });
                    }
                    if (Result.Tables[1].Rows.Count > 0)
                    {
                        State = Convert.ToBoolean(Result.Tables[1].Rows[0][0]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstShiping, State };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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




        #region Seal Cutting 


        public void GetSealCuttingDateId(string Id)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_SealNo", MySqlDbType = MySqlDbType.VarChar, Value = Id });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("getsealcuttingdatebyid", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CwcExim.Areas.Import.Models.PPGSealCuttingDateForReportV2 objseal = new CwcExim.Areas.Import.Models.PPGSealCuttingDateForReportV2();
            try
            {


                if (Result.Read())
                {

                    Status = 1;

                    objseal.GateInDate = Convert.ToString(Result["GateInDate"]);
                    objseal.TranscationDate = Convert.ToString(Result["TranscationDate"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Data = objseal;
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
        public void AddEditSealCutting(SealCuttingV2 Obj, string ChargesBreakupXML, string ChargesXML, int BranchId, int Uid, string OBLXML, string IPAddress)
        {
            try
            {
                string InvoiceNo = "";
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "In_SealCuttingId", MySqlDbType = MySqlDbType.Int32, Value = Obj.SealCuttingId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_SealCuttingNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.SealCuttingNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TransactionDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Obj.TransactionDate) });
                LstParam.Add(new MySqlParameter { ParameterName = "In_BLId", MySqlDbType = MySqlDbType.Int32, Value = Obj.BLId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_BlDetailId", MySqlDbType = MySqlDbType.Int32, Value = Obj.BlDetailId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_BLNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.BLNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_BLDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Obj.BLDate) });
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLType", MySqlDbType = MySqlDbType.Int32, Value = Obj.OBLType });
                LstParam.Add(new MySqlParameter { ParameterName = "In_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = Obj.ContainerId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.ContainerNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_size", MySqlDbType = MySqlDbType.VarChar, Value = Obj.size });
                LstParam.Add(new MySqlParameter { ParameterName = "In_GateInDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Obj.GateInDate) });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = Obj.CFSCode });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CustomSealId", MySqlDbType = MySqlDbType.Int32, Value = Obj.CustomSealId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.CustomSealNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_GodownId", MySqlDbType = MySqlDbType.Int32, Value = Obj.GodownId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_GodownNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.GodownNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CHAShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = Obj.CHAShippingLineId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CHAShippingLine", MySqlDbType = MySqlDbType.VarChar, Value = Obj.CHAShippingLine });
                LstParam.Add(new MySqlParameter { ParameterName = "In_FolioNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.FolioNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_Balance", MySqlDbType = MySqlDbType.Decimal, Value = Obj.Balance });
                LstParam.Add(new MySqlParameter { ParameterName = "In_GroundRent", MySqlDbType = MySqlDbType.Decimal, Value = Obj.GroundRent });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CGST", MySqlDbType = MySqlDbType.Decimal, Value = Obj.CGST });
                LstParam.Add(new MySqlParameter { ParameterName = "In_SGST", MySqlDbType = MySqlDbType.Decimal, Value = Obj.SGST });
                LstParam.Add(new MySqlParameter { ParameterName = "In_IGST", MySqlDbType = MySqlDbType.Decimal, Value = Obj.IGST });
                LstParam.Add(new MySqlParameter { ParameterName = "In_IGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Obj.IGSTPer });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Obj.CGSTPer });
                LstParam.Add(new MySqlParameter { ParameterName = "In_SGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Obj.SGSTPer });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = Obj.TotalAmount });

                LstParam.Add(new MySqlParameter { ParameterName = "In_CBTDTF", MySqlDbType = MySqlDbType.Decimal, Value = Obj.CBTDTF });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DTFCGST", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DTFCGST });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DTFSGST", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DTFSGST });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DTFIGST", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DTFIGST });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DTFCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DTFCGSTPer });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DTFSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DTFSGSTPer });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DTFIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DTFIGSTPer });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DTFTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DTFTotalAmount });

                LstParam.Add(new MySqlParameter { ParameterName = "In_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = Obj.TotalTaxable });
                LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = Obj.TotalCGST });
                LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = Obj.TotalSGST });
                LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = Obj.TotalIGST });
                LstParam.Add(new MySqlParameter { ParameterName = "In_InvoiceAmount", MySqlDbType = MySqlDbType.Decimal, Value = Obj.InvoiceAmt });
                LstParam.Add(new MySqlParameter { ParameterName = "In_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = Obj.InvoiceId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
                LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLXML", MySqlDbType = MySqlDbType.Text, Value = OBLXML });
                LstParam.Add(new MySqlParameter { ParameterName = "In_ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });
                LstParam.Add(new MySqlParameter { ParameterName = "In_Cargo", MySqlDbType = MySqlDbType.Int32, Value = Obj.CargoTypeId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = Obj.SEZ });
                LstParam.Add(new MySqlParameter { ParameterName = "in_IPAddress", MySqlDbType = MySqlDbType.VarChar, Value = IPAddress });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = InvoiceNo });

                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("AddEditSealCuttingV2", CommandType.StoredProcedure, DParam, out InvoiceNo);
                _DBResponse = new DatabaseResponse();
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Seal Cutting Save Successfully";
                    _DBResponse.Data = InvoiceNo;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Seal Cutting Updated Successfully";
                    _DBResponse.Data = InvoiceNo;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Fail To Save as SD Balance is less";
                    _DBResponse.Data = null;
                }
                else if (Result == 6)
                {
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Seal Cutting For This Container Already Done";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Fail To Save Seal Cutting";
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
        public void ListOfGodown()
        {
            int Status = 0;
            int uid = ((CwcExim.Models.Login)HttpContext.Current.Session["LoginUser"]).Uid;

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstGodownAccessRights", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.GodownList> lstGodownList = new List<Areas.Import.Models.GodownList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGodownList.Add(new Areas.Import.Models.GodownList
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        GodownNo = Convert.ToString(Result["GodownName"]),

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
        public void ListOfGodownRights(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "In_UserId", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstGodownRights", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.GodownList> lstGodownList = new List<Areas.Import.Models.GodownList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGodownList.Add(new Areas.Import.Models.GodownList
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        GodownNo = Convert.ToString(Result["GodownName"]),

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

        public void ListOfContainer()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerDetailForSealCuttingV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.SealCuttingV2> lstContainerList = new List<Areas.Import.Models.SealCuttingV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstContainerList.Add(new Areas.Import.Models.SealCuttingV2
                    {
                        ContainerId = Convert.ToInt32(Result["ContainerId"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        OBLId = Convert.ToInt32(Result["OBLId"]),
                        size = Convert.ToString(Result["ContainerSize"]),
                        GateInDate = Convert.ToString(Result["EntryDateTime"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        CustomSealNo = Convert.ToString(Result["CustomSealNo"]),
                        LCLFCL = Result["LCLFCL"].ToString(),
                        impobldtlId = Convert.ToInt32(Result["impobldtlId"]),
                        CargoTypeId = Convert.ToInt32(Result["CargoType"]),
                        CHAShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        CHAShippingLine = Convert.ToString(Result["ShippingLine"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstContainerList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void ListOfBL(int OBLId, int impobldtlId, string OBLFCLLCL)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBLId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = OBLId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_impobldtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = impobldtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBLFCLLCL", MySqlDbType = MySqlDbType.VarChar, Value = OBLFCLLCL });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBLDetailForSealCutting", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.SealCuttingV2> lstSealCuttingList = new List<Areas.Import.Models.SealCuttingV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstSealCuttingList.Add(new Areas.Import.Models.SealCuttingV2
                    {
                        BLId = Convert.ToInt32(Result["ImpOblEntryHeaderId"]),
                        BlDetailId = Convert.ToInt32(Result["BlDetailId"]),
                        BLNo = Convert.ToString(Result["OBL_No"]),
                        BLDate = Convert.ToString(Result["OBL_Date"]),
                        CargoTypeId = Convert.ToInt32(Result["CargoTypeId"]),
                        CargoType = Convert.ToString(Result["CargoType"]),
                        NO_PKG = Convert.ToString(Result["NO_PKG"]),
                        GR_WT = Convert.ToDecimal(Result["GR_WT"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstSealCuttingList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void ListOfCHAShippingLine(int Page, string Code)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.String, Value = Code });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetCHAShippingLineForSealCuttingV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.SealCuttingCHAV2> lstCHAList = new List<Areas.Import.Models.SealCuttingCHAV2>();
            try
            {
                Status = 1;
                bool State = false;
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    var obj = new Areas.Import.Models.SealCuttingCHAV2();

                    obj.CHAShippingLineId = Convert.ToInt32(dr["EximTraderId"]);
                    obj.CHAShippingLine = Convert.ToString(dr["EximTraderName"]);
                    obj.FolioNo = Convert.ToString(dr["FolioNo"]);
                    obj.Balance = Convert.ToDecimal(dr["Balance"]);
                    obj.PartyCode = Convert.ToString(dr["EximTraderAlias"]);

                    lstCHAList.Add(obj);
                }
                if (Result.Tables[1].Rows.Count > 0)
                {
                    State = Convert.ToBoolean(Result.Tables[1].Rows[0][0]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { LstParty= lstCHAList, State };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = new { LstParty= lstCHAList, State };
                }
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

        internal void GetInvoiceDtlForSealCutting(string trancastionDate, string gateInDate, string containerNo, string size, int cHAShippingLineId, string CFSCode, int oBLType, int CargoType,string SEZ)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (size == "")
            {
                size = "0";
            }
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = cHAShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StartDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(gateInDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(trancastionDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.VarChar, Value = size });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = oBLType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateGroundRentV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                SealCuttingV2 ObjSC = new SealCuttingV2();
                int count = 0;
                while (result.Read())
                {
                    Status = 1;
                    ObjSC.lstPostPaymentChrg.Add(new PostPaymentChargeV2
                    {
                        CGSTAmt = Convert.ToDecimal(result["CGST"]),
                        SGSTAmt = Convert.ToDecimal(result["SGST"]),
                        IGSTAmt = Convert.ToDecimal(result["IGST"]),
                        CGSTPer = Convert.ToDecimal(result["CGSTPer"]),
                        SGSTPer = Convert.ToDecimal(result["SGSTPer"]),
                        IGSTPer = Convert.ToDecimal(result["IGSTPer"]),
                        SACCode = Convert.ToString(result["SacCode"]),
                        Clause = Convert.ToString(result["Clause"]),
                        ChargeType = Convert.ToString(result["ChargeType"]),
                        ChargeName = Convert.ToString(result["ChargeName"]),
                        Taxable = Convert.ToDecimal(result["Taxable"]),
                        Total = Convert.ToDecimal(result["TotalAmount"]),
                        OperationId = Convert.ToInt32(result["OperationId"]),
                    });

                }

                ObjSC.TotalTaxable = ObjSC.lstPostPaymentChrg.Sum(m => m.Taxable);
                ObjSC.TotalCGST = ObjSC.lstPostPaymentChrg.Sum(m => m.CGSTAmt);
                ObjSC.TotalSGST = ObjSC.lstPostPaymentChrg.Sum(m => m.SGSTAmt);
                ObjSC.TotalIGST = ObjSC.lstPostPaymentChrg.Sum(m => m.IGSTAmt);
                ObjSC.InvoiceAmt = ObjSC.lstPostPaymentChrg.Sum(m => m.Total);

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjSC.lstObldt.Add(new SealCuttingV2
                        {
                            Balance = Convert.ToDecimal(result["Balance"])
                        });
                    }
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {

                        ObjSC.lstPostPaymentChrgBreakup.Add(new ppgSealPostPaymentChargebreakupdateV2
                        {
                            ChargeId = Convert.ToInt32(result["OperationId"]),
                            Clause = result["ChargeType"].ToString(),
                            ChargeType = result["ChargeType"].ToString(),
                            ChargeName = result["ChargeName"].ToString(),
                            SACCode = result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                            OperationId = Convert.ToInt32(result["OperationId"]),
                            CFSCode = result["CFSCode"].ToString(),
                            Startdate = result["StartDate"].ToString(),
                            EndDate = result["EndDate"].ToString(),

                        });
                    }


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Successful";
                    _DBResponse.Data = ObjSC;
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
                result.Close();
                result.Dispose();

            }
        }

        public void GetListOfSealCuttingDetails(string Module, string InvoiceNo = null, string multi = null, string InvoiceDate = null, int Page=0)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            if (InvoiceDate == "") InvoiceDate = null;
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_Multi", MySqlDbType = MySqlDbType.VarChar, Value = multi });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet ds = DataAccess.ExecuteDataSet("GetListOfSealCuttingForPageV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    List<SealCuttingV2> lstSealCutting = new List<SealCuttingV2>();
                    lstSealCutting = (from DataRow dr in ds.Tables[0].Rows
                                      select new SealCuttingV2()
                                      {
                                          SealCuttingId = Convert.ToInt32(dr["SealCuttingId"]),
                                          SealCuttingNo = Convert.ToString(dr["SealCuttingNo"]),
                                          TransactionDate = Convert.ToString(dr["TranscationDate"].ToString()),
                                          BLNo = dr["BLNo"].ToString(),
                                          BLDate = Convert.ToString(dr["BLDate"].ToString()),
                                          ContainerNo = dr["ContainerNo"].ToString(),
                                          size = Convert.ToString(dr["size"].ToString()),
                                          GateInDate = Convert.ToString(dr["GateInDate"]),
                                          CFSCode = dr["CFSCode"].ToString(),
                                          GodownNo = dr["GodownNo"].ToString(),
                                          InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                                          InvoiceNo = dr["InvoiceNo"].ToString()
                                      }).ToList();
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Successful";
                    _DBResponse.Data = lstSealCutting;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                ds.Dispose();
            }
        }

        public void GetSealCuttingById(int SealCuttingId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_SealCuttingId", MySqlDbType = MySqlDbType.Int32, Value = SealCuttingId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("GetSealCuttingByIdV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            int ctype = 2;
            try
            {
                SealCuttingV2 ObjSC = new SealCuttingV2();
                while (result.Read())
                {
                    Status = 1;
                    ObjSC.SealCuttingId = Convert.ToInt32(result["SealCuttingId"]);
                    ObjSC.SealCuttingNo = Convert.ToString(result["SealCuttingNo"]);
                    ObjSC.TransactionDate = Convert.ToString(result["TranscationDate"].ToString());
                    ObjSC.BLNo = result["BLNo"].ToString();
                    ObjSC.BLDate = Convert.ToString(result["BLDate"].ToString());
                    ObjSC.ContainerNo = result["ContainerNo"].ToString();
                    ObjSC.size = Convert.ToString(result["size"].ToString());
                    ObjSC.GateInDate = Convert.ToString(result["GateInDate"].ToString());
                    ObjSC.CFSCode = result["CFSCode"].ToString();
                    ObjSC.GodownNo = result["GodownNo"].ToString();
                    ObjSC.GodownId = Convert.ToInt32(result["GodownId"]);
                    ObjSC.CHAShippingLineId = Convert.ToInt32(result["CHAShippingLineId"].ToString());
                    ObjSC.CHAShippingLine = result["CHAShippingLine"].ToString();
                    ObjSC.FolioNo = Convert.ToString(result["FolioNo"].ToString());
                    ObjSC.Balance = Convert.ToDecimal(result["Balance"]);
                    ObjSC.GroundRent = Convert.ToDecimal(result["GroundRent"]);
                    ObjSC.CGST = Convert.ToDecimal(result["CGST"]);
                    ObjSC.SGST = Convert.ToDecimal(result["SGST"]);
                    ObjSC.IGST = Convert.ToDecimal(result["IGST"]);
                    ObjSC.TotalAmount = Convert.ToDecimal(result["TotalAmount"]);
                    ObjSC.CustomSealNo = Convert.ToString(result["CustomSealNo"]);
                    ObjSC.InvoiceId = Convert.ToInt32(result["InvoiceId"]);
                    ObjSC.InvoiceNo = Convert.ToString(result["InvoiceNo"]);
                    ObjSC.OBLType = Convert.ToString(result["OBLType"]);
                    ObjSC.DisplayOBLType = Convert.ToString(result["DisplayOBLType"]);
                    ObjSC.CGSTPer = Convert.ToDecimal(result["CGSTPer"]);
                    ObjSC.SGSTPer = Convert.ToDecimal(result["SGSTPer"]);
                    ObjSC.IGSTPer = Convert.ToDecimal(result["IGSTPer"]);
                    ObjSC.LCLFCL = Convert.ToString(result["LCLFCL"]);

                    ObjSC.CBTDTF = Convert.ToDecimal(result["CBTDTF"]);
                    ObjSC.DTFCGST = Convert.ToDecimal(result["CBTDTFCGST"]);
                    ObjSC.DTFSGST = Convert.ToDecimal(result["CBTDTFSGST"]);
                    ObjSC.DTFIGST = Convert.ToDecimal(result["CBTDTFIGST"]);
                    ObjSC.DTFTotalAmount = Convert.ToDecimal(result["CBTDTFTotal"]);
                    ObjSC.DTFCGSTPer = Convert.ToDecimal(result["CBTDTFCGSTPer"]);
                    ObjSC.DTFSGSTPer = Convert.ToDecimal(result["CBTDTFSGSTPer"]);
                    ObjSC.DTFIGSTPer = Convert.ToDecimal(result["CBTDTFIGSTPer"]);

                    ObjSC.InvoiceAmt = Convert.ToDecimal(result["InvoiceAmt"]);
                    ObjSC.TotalTaxable = ObjSC.GroundRent + ObjSC.CBTDTF;
                    ObjSC.TotalCGST = ObjSC.CGST + ObjSC.DTFCGST;
                    ObjSC.TotalSGST = ObjSC.SGST + ObjSC.DTFIGST;
                    ObjSC.TotalIGST = ObjSC.IGST + ObjSC.DTFIGST;

                }
                //IList<Areas.Import.Models.SealCutting> lstOblList = new List<Areas.Import.Models.SealCutting>();
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        Status = 1;

                        ObjSC.lstObldt.Add(new Areas.Import.Models.SealCuttingV2
                        {

                            BLId = Convert.ToInt32(result["ImpOblEntryHeaderId"]),
                            BlDetailId = Convert.ToInt32(result["BlDetailId"]),
                            BLNo = Convert.ToString(result["OBL_No"]),
                            BLDate = Convert.ToString(result["OBL_Date"]),
                            CargoTypeId = Convert.ToInt32(result["CargoTypeId"]),
                            CargoType = Convert.ToString(result["CargoType"]),
                            NO_PKG = Convert.ToString(result["NO_PKG"]),
                            GR_WT = Convert.ToDecimal(result["GR_WT"])
                        });
                    }
                }
                if (result.NextResult())
                {
                    List<ppgSealPostPaymentChargebreakupdateV2> lstdtl = new List<ppgSealPostPaymentChargebreakupdateV2>();
                    while (result.Read())
                    {
                        lstdtl.Add(new ppgSealPostPaymentChargebreakupdateV2
                        {
                            ChargeId = Convert.ToInt32(result["OperationId"]),
                            Clause = result["ChargeType"].ToString(),
                            ChargeType = result["ChargeType"].ToString(),
                            ChargeName = result["ChargeName"].ToString(),
                            SACCode = result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                            OperationId = Convert.ToInt32(result["OperationId"]),
                            CFSCode = result["CFSCode"].ToString(),
                            Startdate = result["StartDate"].ToString(),
                            EndDate = result["EndDate"].ToString(),
                        });
                    }
                    if (lstdtl.Count > 0)
                    {
                        ObjSC.lstPostPaymentChrgBreakupAmt = Newtonsoft.Json.JsonConvert.SerializeObject(lstdtl);
                    }
                }
                if (result.NextResult())
                {
                    List<PostPaymentChargeV2> lstPostPaymentChrg = new List<PostPaymentChargeV2>();
                    while (result.Read())
                    {
                        ObjSC.lstPostPaymentChrg.Add(new PostPaymentChargeV2
                        {
                            CGSTAmt = Convert.ToDecimal(result["CGSTAmt"]),
                            SGSTAmt = Convert.ToDecimal(result["SGSTAmt"]),
                            IGSTAmt = Convert.ToDecimal(result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(result["CGSTPer"]),
                            SGSTPer = Convert.ToDecimal(result["SGSTPer"]),
                            IGSTPer = Convert.ToDecimal(result["IGSTPer"]),
                            SACCode = Convert.ToString(result["SacCode"]),
                            Clause = Convert.ToString(result["Clause"]),
                            ChargeType = Convert.ToString(result["ChargeType"]),
                            ChargeName = Convert.ToString(result["ChargeName"]),
                            Taxable = Convert.ToDecimal(result["Taxable"]),
                            Total = Convert.ToDecimal(result["Total"])
                        });
                    }
                    ObjSC.TotalTaxable = ObjSC.lstPostPaymentChrg.Sum(m => m.Taxable);
                    ObjSC.TotalCGST = ObjSC.lstPostPaymentChrg.Sum(m => m.CGSTAmt);
                    ObjSC.TotalSGST = ObjSC.lstPostPaymentChrg.Sum(m => m.SGSTAmt);
                    ObjSC.TotalIGST = ObjSC.lstPostPaymentChrg.Sum(m => m.IGSTAmt);
                    ObjSC.InvoiceAmt = ObjSC.lstPostPaymentChrg.Sum(m => m.Total);
                    if (ObjSC.lstPostPaymentChrg.Count > 0)
                    {
                        ObjSC.lstPostPaymentChrgAmt = Newtonsoft.Json.JsonConvert.SerializeObject(ObjSC.lstPostPaymentChrg);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Successful";
                    _DBResponse.Data = ObjSC;
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
                result.Close();
                result.Dispose();
            }
        }

        public void DeleteSealCutting(int SealCuttingId)
        {
            int RetValue = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_SealCuttingId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(SealCuttingId) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = RetValue });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteSealCuttingV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Seal Cutting Deleted Successfully";
                    _DBResponse.Data = null;
                }
                if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot delete as Seal Cutting used in Tally Sheet";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot delete as invoice created";
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
        public void SealCuttingEmailUpdate(string InvoiceNo)
        {
            try
            {
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });

                IDataParameter[] DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("SealCuttingEmailUpdateV2", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Failed";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Fail To Save as SD Balance is less";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Fail To Save Seal Cutting";
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


        #region Seal Cutting Invoices
        public void GetSCInvoiceDetailsForPrint(int InvoiceId, string InvoiceType = "IMPSC")
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceType });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSCInvoiceDetailsForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PpgInvoiceSealCuttingV2 objPaymentSheet = new PpgInvoiceSealCuttingV2();
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
                        objPaymentSheet.LstContainersSealCutting.Add(new PpgInvoiceContainerSealCuttingV2()
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
                        objPaymentSheet.LstChargesSealCutting.Add(new PpgInvoiceChargeSealCuttingV2()
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
        #endregion

        #region Internal Movement App
        public void GetAllInternalMovementApp(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInternalMovementappV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_Internal_MovementAppV2> LstInternalMovement = new List<PPG_Internal_MovementAppV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new PPG_Internal_MovementAppV2
                    {
                        MovementNo = Result["MovementNo"].ToString(),
                        BOENo = Result["BOENo"].ToString(),
                        MovementId = Convert.ToInt32(Result["MovementId"]),
                        MovementDate = Result["MovementDate"].ToString(),
                        OldGodownName = Result["FromGodown"].ToString(),
                        NewGodownName = Result["ToGodown"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInternalMovement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void AddEditInvoiceMovementApp(PPG_Internal_MovementAppV2 ObjPostPaymentSheet, int BranchId, int Uid, string Module)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPostPaymentSheet.MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPostPaymentSheet.DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjPostPaymentSheet.MovementDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjPostPaymentSheet.BOEDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPostPaymentSheet.NoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.GrossWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPostPaymentSheet.FromGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPostPaymentSheet.ToGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.OldLocationIds });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.OldLctnNames });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.LocationName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ModuleNameMovement", MySqlDbType = MySqlDbType.VarChar, Value = "Import" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInternalMovementappV2", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Internal Movement Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Internal Movement Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Can't updated next step already done";
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
        public void GetInternalMovementApp(int MovementId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInternalMovementappV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPG_Internal_MovementAppV2 ObjInternalMovement = new PPG_Internal_MovementAppV2();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjInternalMovement.MovementId = Convert.ToInt32(Result["MovementId"]);
                    ObjInternalMovement.MovementNo = Result["MovementNo"].ToString();
                    ObjInternalMovement.MovementDate = Result["MovementDate"].ToString();
                    ObjInternalMovement.CargoDescription = Result["CargoDescription"].ToString();
                    ObjInternalMovement.BOENo = Result["BOENo"].ToString();
                    ObjInternalMovement.BOEDate = Result["BOEDate"].ToString();
                    ObjInternalMovement.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjInternalMovement.GrossWeight = Convert.ToInt32(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjInternalMovement.OldLocationIds = Result["OldLocationIds"].ToString();
                    ObjInternalMovement.OldLctnNames = Result["OldLctnNames"].ToString();
                    ObjInternalMovement.LocationId = Result["NewLocationIds"].ToString();
                    ObjInternalMovement.LocationName = Result["NewLctnNames"].ToString();
                    ObjInternalMovement.FromGodownId = Convert.ToInt32(Result["FromGodownId"]);
                    ObjInternalMovement.ToGodownId = Convert.ToInt32(Result["ToGodownId"]);
                    ObjInternalMovement.NewGodownName = Result["NewGodownName"].ToString();
                    ObjInternalMovement.OldGodownName = Result["OldGodownName"].ToString();
                    ObjInternalMovement.DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]);
                    ObjInternalMovement.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryDtlId"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjInternalMovement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void DelInternalMovementApp(int MovementId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Internal Movement Details Deleted Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Internal Movement Details As It Exists In Another Page";
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
        public void AddEditImpInternalMovementApp(PPG_Internal_MovementAppV2 ObjInternalMovement)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.DestuffingEntryDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.MovementDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.BOEDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.NoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.GrossWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.FromGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.ToGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.OldLocationIds });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.OldLctnNames });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLocationIds", MySqlDbType = MySqlDbType.Int32, Value = ObjInternalMovement.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.LocationName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjInternalMovement.MovementId == 0 ? "Internal Movement Details Saved Successfully" : "Internal Movement  Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Internal Movement  Details Already Exists";
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
        public void GetBOENoForInternalMovementApp()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBOENoForInternalMovementV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_Internal_MovementAppV2> LstInternalMovement = new List<PPG_Internal_MovementAppV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstInternalMovement.Add(new PPG_Internal_MovementAppV2
                    {

                        BOENo = Result["BOENo"].ToString(),
                        DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"])
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInternalMovement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetLocationForInternalMovementApp(int gid)
        {
            int Status = 0;

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Godown_id", MySqlDbType = MySqlDbType.Int32, Value = gid });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLocationInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_Internal_MovementAppV2> LstInternalMovement = new List<PPG_Internal_MovementAppV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new PPG_Internal_MovementAppV2
                    {
                        LocationName = Result["LocationName"].ToString(),
                        LocationId = (Result["LocationId"]).ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInternalMovement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetBOENoDetForMovementApp(int DestuffingEntryDtlId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBOENoForInternalMovementV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPG_Internal_MovementAppV2 ObjInternalMovement = new PPG_Internal_MovementAppV2();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjInternalMovement.BOENo = Result["BOENo"].ToString();
                    ObjInternalMovement.BOEDate = Result["BOEDate"].ToString();
                    ObjInternalMovement.CargoDescription = Result["CargoDescription"].ToString();
                    ObjInternalMovement.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjInternalMovement.GrossWeight = Convert.ToInt32(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjInternalMovement.OldLocationIds = Result["OldLocationIds"].ToString();
                    ObjInternalMovement.OldLctnNames = Result["OldLctnNames"].ToString();
                    ObjInternalMovement.FromGodownId = Convert.ToInt32(Result["FromGodownId"]);
                    ObjInternalMovement.OldGodownName = Result["OldGodownName"].ToString();
                    ObjInternalMovement.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"].ToString());

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjInternalMovement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region Internal Movement
        public void GetAllInternalMovement(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page});
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpInternalMovementV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_Internal_MovementV2> LstInternalMovement = new List<PPG_Internal_MovementV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new PPG_Internal_MovementV2
                    {
                        MovementNo = Result["MovementNo"].ToString(),
                        MovementDate= Result["MovementDate"].ToString(),
                        BOENo = Result["BOENo"].ToString(),
                        OldGodownName= Result["FromGodown"].ToString(),
                        NewGodownName = Result["ToGodown"].ToString(),
                        MovementId = Convert.ToInt32(Result["MovementId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInternalMovement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void AddEditImpInternalMovement(PPG_Internal_MovementV2 ObjPostPaymentSheet, int BranchId, int Uid, string Module)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPostPaymentSheet.MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPostPaymentSheet.DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjPostPaymentSheet.MovementDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjPostPaymentSheet.BOEDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPostPaymentSheet.NoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.GrossWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPostPaymentSheet.FromGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPostPaymentSheet.ToGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.OldLocationIds });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.OldLctnNames });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.LocationName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ModuleNameMovement", MySqlDbType = MySqlDbType.VarChar, Value = "Import" });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TransferId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.TransferId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Status", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Status });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });


            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditImpInternalMovementV2", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Internal Movement Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Internal Movement Updated Successfully";
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
        public void GetInternalMovement(int MovementId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpInternalMovementV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPG_Internal_MovementV2 ObjInternalMovement = new PPG_Internal_MovementV2();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjInternalMovement.MovementId = Convert.ToInt32(Result["MovementId"]);
                    ObjInternalMovement.MovementNo = Result["MovementNo"].ToString();
                    ObjInternalMovement.MovementDate = Result["MovementDate"].ToString();
                    ObjInternalMovement.CargoDescription = Result["CargoDescription"].ToString();
                    ObjInternalMovement.BOENo = Result["BOENo"].ToString();
                    ObjInternalMovement.BOEDate = Result["BOEDate"].ToString();
                    ObjInternalMovement.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjInternalMovement.GrossWeight = Convert.ToInt32(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjInternalMovement.OldLocationIds = Result["OldLocationIds"].ToString();
                    ObjInternalMovement.OldLctnNames = Result["OldLctnNames"].ToString();
                    ObjInternalMovement.LocationId = Convert.ToString(Result["NewLocationIds"].ToString());
                    ObjInternalMovement.LocationName = Result["NewLctnNames"].ToString();
                    ObjInternalMovement.FromGodownId = Convert.ToInt32(Result["FromGodownId"]);
                    ObjInternalMovement.ToGodownId = Convert.ToInt32(Result["ToGodownId"]);
                    ObjInternalMovement.NewGodownName = Result["NewGodownName"].ToString();
                    ObjInternalMovement.OldGodownName = Result["OldGodownName"].ToString();
                    ObjInternalMovement.DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]);
                    ObjInternalMovement.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryDtlId"]);
                    ObjInternalMovement.TransferNo = Result["TransferNo"].ToString();
                    ObjInternalMovement.TransferDate = Result["TransferDate"].ToString();
                    ObjInternalMovement.Status = Result["ApprovedRejected"].ToString();
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjInternalMovement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetBOENoForInternalMovement()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransferId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetTransferNoListV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_Internal_MovementV2> LstInternalMovement = new List<PPG_Internal_MovementV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstInternalMovement.Add(new PPG_Internal_MovementV2
                    {
                        BOENo = Result["BOENo"].ToString(),
                        DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                        TransferId = Convert.ToInt32(Result["TransferId"]),
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInternalMovement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetLocationForInternalMovement(int gid)
        {
            int Status = 0;

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Godown_id", MySqlDbType = MySqlDbType.Int32, Value = gid });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLocationInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_Internal_Movement> LstInternalMovement = new List<PPG_Internal_Movement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new PPG_Internal_Movement
                    {
                        LocationName = Result["LocationName"].ToString(),
                        LocationId = Convert.ToInt32(Result["LocationId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInternalMovement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetBOENoDetForMovement(int TransferId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransferId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TransferId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetTransferNoListV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPG_Internal_MovementV2 ObjInternalMovement = new PPG_Internal_MovementV2();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjInternalMovement.BOENo = Result["BOENo"].ToString();
                    ObjInternalMovement.BOEDate = Result["BOEDate"].ToString();
                    ObjInternalMovement.CargoDescription = Result["CargoDescription"].ToString();
                    ObjInternalMovement.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjInternalMovement.GrossWeight = Convert.ToInt32(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjInternalMovement.OldLocationIds = Result["OldLocationIds"].ToString();
                    ObjInternalMovement.OldLctnNames = Result["OldLctnNames"].ToString();
                    ObjInternalMovement.FromGodownId = Convert.ToInt32(Result["FromGodownId"]);
                    ObjInternalMovement.OldGodownName = Result["OldGodownName"].ToString();
                    ObjInternalMovement.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"].ToString());
                    ObjInternalMovement.TransferId = Convert.ToInt32(Result["TransferId"]);
                    ObjInternalMovement.TransferNo = Convert.ToString(Result["TransferNo"]);
                    ObjInternalMovement.TransferDate = Convert.ToString(Result["TransferDate"]);
                    ObjInternalMovement.NewLocationIds = Result["NewLocationIds"].ToString();
                    ObjInternalMovement.NewLctnNames = Result["NewLctnNames"].ToString();
                    ObjInternalMovement.ToGodownId = Convert.ToInt32(Result["ToGodownId"]);
                    ObjInternalMovement.NewGodownName = Result["NewGodownName"].ToString();
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjInternalMovement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void MenuAccessRight(int RoleId, int BranchId, int ModuleId, int MenuId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_MenuId", MySqlDbType = MySqlDbType.Int32, Value = MenuId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ModuleId", MySqlDbType = MySqlDbType.Int32, Value = ModuleId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Branch_Id", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
              
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetMenuWiseAccessRight", CommandType.StoredProcedure, dpram);
            IList<PPG_Menu> lstMenu = new List<PPG_Menu>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (result.Read())
                {

                    lstMenu.Add(new PPG_Menu
                    {
                        CanAdd = Convert.ToInt32(result["CanAdd"]),
                        CanEdit = Convert.ToInt32(result["CanEdit"]),
                        CanDelete = Convert.ToInt32(result["CanDelete"]),
                        CanView = Convert.ToInt32(result["CanView"])

                    });
                }
                _DBResponse.Data = new { lstMenu };
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";

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


        #region OBL SPILT

        public void GetOBLListForSpilt()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {


                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLNoForSpilt", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                List<Ppg_OBLSpiltList> LstOBLSpilt = new List<Ppg_OBLSpiltList>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        LstOBLSpilt.Add(new Ppg_OBLSpiltList
                        {
                            SpiltOBLNo = Convert.ToString(dr["BOLNo"]),
                            SpiltOBLDate = Convert.ToString(dr["BOLDate"]),
                            IsFCL = Convert.ToInt32(dr["isFCL"])
                        });
                    }
                }

                if (Status == 1)
                {


                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstOBLSpilt;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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


        public void GetOBLDetailsForSpilt(string OBL, string Date, int isFCL)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {


                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "in_OBL", MySqlDbType = MySqlDbType.VarString, Value = OBL });
                LstParam.Add(new MySqlParameter { ParameterName = "in_OBLDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(Date) });
                LstParam.Add(new MySqlParameter { ParameterName = "in_isFCL", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(isFCL) });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLNoDetailsForSpilt", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                Ppg_OBLSpilt LstOBLSpilt = new Ppg_OBLSpilt();

                if (Result != null && Result.Tables[0].Rows.Count > 0)
                {
                    Status = 1;

                    LstOBLSpilt.OBLNo = Convert.ToString(Result.Tables[0].Rows[0]["OBL_No"]);
                    LstOBLSpilt.SpiltOBLDate = Convert.ToString(Result.Tables[0].Rows[0]["OBL_Date"]);
                    LstOBLSpilt.HeaderId = Convert.ToInt32(Result.Tables[0].Rows[0]["HeaderID"]);
                    LstOBLSpilt.DetailsId = Convert.ToInt32(Result.Tables[0].Rows[0]["DetailsId"]);
                    LstOBLSpilt.NoOfPkg = Convert.ToDecimal(Result.Tables[0].Rows[0]["PKG"]);
                    LstOBLSpilt.GRWT = Convert.ToDecimal(Result.Tables[0].Rows[0]["WT"]);
                    LstOBLSpilt.CargoDesc = Convert.ToString(Result.Tables[0].Rows[0]["CargoDescription"]);
                    LstOBLSpilt.CommodityId = Convert.ToString(Result.Tables[0].Rows[0]["CommodityId"]);
                    LstOBLSpilt.CommodityName = Convert.ToString(Result.Tables[0].Rows[0]["CommodityName"]);
                    LstOBLSpilt.ImporterId = Convert.ToInt32(Result.Tables[0].Rows[0]["ImporterId"]);
                    LstOBLSpilt.ImporterName = Convert.ToString(Result.Tables[0].Rows[0]["EximTraderName"]);
                    LstOBLSpilt.PKG_Type = Convert.ToString(Result.Tables[0].Rows[0]["PKG_TYPE"]);
                    LstOBLSpilt.IGMDate = Convert.ToString(Result.Tables[0].Rows[0]["IGM_Date"]);
                    LstOBLSpilt.IGMNo = Convert.ToString(Result.Tables[0].Rows[0]["IGM_No"]);
                    LstOBLSpilt.LineNo = Convert.ToString(Result.Tables[0].Rows[0]["LINE_NO"]);
                    LstOBLSpilt.SMPTNo = Convert.ToString(Result.Tables[0].Rows[0]["SMTPNo"]);
                    LstOBLSpilt.SMPTDate = Convert.ToString(Result.Tables[0].Rows[0]["SMTP_Date"]);


                }

                if (Status == 1)
                {


                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstOBLSpilt;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void AddEditOBLSpilt(Ppg_OBLSpilt Obj,  string ChargesXML, int Uid,string SpiltDate,int IsFCL)
        {
            try
            {

                DateTime dtSpiltDate = DateTime.ParseExact(SpiltDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                String varSpiltDate = dtSpiltDate.ToString("yyyy-MM-dd");

                DateTime dtOBLDate = DateTime.ParseExact(Obj.SpiltOBLDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                String OBLDate = dtOBLDate.ToString("yyyy-MM-dd");

                String SMPTDate = null;
                if (Obj.SMPTDate!=null)
                {
                    DateTime dtSMPTDate = DateTime.ParseExact(Obj.SMPTDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                     SMPTDate = dtSMPTDate.ToString("yyyy-MM-dd");
                }

                String IGMDate = null;
                if (Obj.IGMDate != null)
                {
                    DateTime dtIGMDate = DateTime.ParseExact(Obj.IGMDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                     IGMDate = dtIGMDate.ToString("yyyy-MM-dd");

                }



                string SpiltNo = "";
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_SpiltDate", MySqlDbType = MySqlDbType.DateTime, Value =Convert.ToDateTime(varSpiltDate) });
                LstParam.Add(new MySqlParameter { ParameterName = "in_isFCL", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(IsFCL) });
                LstParam.Add(new MySqlParameter { ParameterName = "in_OBLNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.OBLNo });
                LstParam.Add(new MySqlParameter { ParameterName = "in_OBLDate", MySqlDbType = MySqlDbType.DateTime, Value =Convert.ToDateTime(OBLDate) });
                LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDesc", MySqlDbType = MySqlDbType.VarChar, Value = Obj.CargoDesc });
                LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityName", MySqlDbType = MySqlDbType.VarChar, Value = Obj.CommodityName });
                LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Obj.CommodityId) });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Pkg", MySqlDbType = MySqlDbType.Decimal, Value = Obj.NoOfPkg });
                LstParam.Add(new MySqlParameter { ParameterName = "in_GrWT", MySqlDbType = MySqlDbType.Decimal, Value = Obj.GRWT });
                LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.VarChar, Value = Obj.ImporterId });
                LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterName", MySqlDbType = MySqlDbType.VarChar, Value = Obj.ImporterName });
                LstParam.Add(new MySqlParameter { ParameterName = "in_LineNo", MySqlDbType = MySqlDbType.VarChar, Value =Obj.LineNo });
                LstParam.Add(new MySqlParameter { ParameterName = "in_SMPTNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.SMPTNo });
                LstParam.Add(new MySqlParameter { ParameterName = "in_SMPTDate", MySqlDbType = MySqlDbType.DateTime, Value = SMPTDate });
                LstParam.Add(new MySqlParameter { ParameterName = "in_IGMNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.IGMNo });
                LstParam.Add(new MySqlParameter { ParameterName = "in_IGMDate", MySqlDbType = MySqlDbType.DateTime, Value = IGMDate});
                LstParam.Add(new MySqlParameter { ParameterName = "in_HeaderId", MySqlDbType = MySqlDbType.Int32, Value = Obj.HeaderId });
                LstParam.Add(new MySqlParameter { ParameterName = "in_DetailsId", MySqlDbType = MySqlDbType.Int32, Value = Obj.DetailsId });
                LstParam.Add(new MySqlParameter { ParameterName = "in_inXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
                LstParam.Add(new MySqlParameter { ParameterName = "in_CRBy", MySqlDbType = MySqlDbType.VarChar, Value = Uid });               
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = SpiltNo });

                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("AddEditOBLSpilt", CommandType.StoredProcedure, DParam,out SpiltNo);
                _DBResponse = new DatabaseResponse();
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "OBL Split Save Successfully";
                    _DBResponse.Data = SpiltNo;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "OBL Split Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "";
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


        public void GetSpiltOBLDetails(string OBLNo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {


                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "IN_id", MySqlDbType = MySqlDbType.Int32, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_OBLNo", MySqlDbType = MySqlDbType.VarChar, Value = OBLNo });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLSpiltDetails", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                List<Ppg_OBLSpilt> LstOBLSpilt = new List<Ppg_OBLSpilt>();

                if (Result != null && Result.Tables[0].Rows.Count > 0)
                {


                    Status = 1;
                    foreach(DataRow dr in Result.Tables[0].Rows)
                    {
                        LstOBLSpilt.Add(new Ppg_OBLSpilt
                        {
                            SpiltID = Convert.ToInt32(dr["SpiltId"]),
                            SpiltNo = Convert.ToString(dr["SpiltNo"]),
                            SpiltDate = Convert.ToString(dr["SpiltDate"]),
                            OBLNo = Convert.ToString(dr["OBLNo"]),
                            OBLNoDate = Convert.ToString(dr["OBLDate"]),
                            SpiltOBLNo = Convert.ToString(dr["SpiltOBL"]),
                            SpiltOBLDate = Convert.ToString(dr["SpiltOblDate"]),
                        });
                    }

                  


                }

                if (Status == 1)
                {


                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstOBLSpilt;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetViewSpiltOBLDetails(int id)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {


                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "IN_id", MySqlDbType = MySqlDbType.Int32, Value =Convert.ToInt32(id) });
                LstParam.Add(new MySqlParameter { ParameterName = "in_OBLNo", MySqlDbType = MySqlDbType.VarChar, Value = "" });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLSpiltDetails", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                Ppg_OBLSpilt LstOBLSpilt = new Ppg_OBLSpilt();

                if (Result != null && Result.Tables[0].Rows.Count > 0)
                {


                    Status = 1;

                    LstOBLSpilt.CargoDesc = Convert.ToString(Result.Tables[0].Rows[0]["CargoDesc"]);
                    LstOBLSpilt.CommodityName = Convert.ToString(Result.Tables[0].Rows[0]["CommodityName"]);
                    LstOBLSpilt.GRWT = Convert.ToDecimal(Result.Tables[0].Rows[0]["WT"]);
                    LstOBLSpilt.ImporterName = Convert.ToString(Result.Tables[0].Rows[0]["ImporterName"]);
                    LstOBLSpilt.LineNo = Convert.ToString(Result.Tables[0].Rows[0]["LineNo"]);
                    LstOBLSpilt.NoOfPkg = Convert.ToDecimal(Result.Tables[0].Rows[0]["PKG"]);
                    LstOBLSpilt.SpiltNo = Convert.ToString(Result.Tables[0].Rows[0]["SpiltNo"]);
                    LstOBLSpilt.SpiltDate = Convert.ToString(Result.Tables[0].Rows[0]["SpiltDate"]);
                    LstOBLSpilt.OBLNo = Convert.ToString(Result.Tables[0].Rows[0]["OBLNo"]);
                    LstOBLSpilt.OBLNoDate = Convert.ToString(Result.Tables[0].Rows[0]["OBLDate"]);
                    LstOBLSpilt.SMPTNo = Convert.ToString(Result.Tables[0].Rows[0]["SMPTNo"]);
                    LstOBLSpilt.SMPTDate = Convert.ToString(Result.Tables[0].Rows[0]["SMPTDate"]);
                    LstOBLSpilt.DetailsId = 0;
                    LstOBLSpilt.HeaderId = 0;
                    LstOBLSpilt.ImporterId = 0;
                    LstOBLSpilt.IsFCL = 0;
                    LstOBLSpilt.SpiltID = 0;


                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        LstOBLSpilt.lstSpiltDetails.Add(new Ppg_OBLSpiltDetails
                        {
                            SpiltCargoDesc = Convert.ToString(dr["SpiltCargoDesc"]),
                            SpiltCommodityName = Convert.ToString(dr["SpiltCommodityName"]),
                            SpiltImporter = Convert.ToString(dr["ImporterName"]),
                            SpiltWT = Convert.ToDecimal(dr["SpiltWT"]),
                            SpiltSMPTNo = Convert.ToString(dr["SpiltSmtpNo"]),
                            SpiltSMPTDate = Convert.ToString(dr["SpiltSmtpDate"]),
                            SpiltPkg = Convert.ToDecimal(dr["SpiltPkg"]),
                            SpiltLineNo = Convert.ToString(dr["SpiltLineNo"]),
                            SpiltOBL = Convert.ToString(dr["SpiltOBL"]),
                            SpiltOBLDate = Convert.ToString(dr["SpiltOblDate"]),
                             SpiltCommodityId=0,
                              SpiltImporterID=0
                             


                        });
                    }






                }

                if (Status == 1)
                {


                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstOBLSpilt;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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



        public void DeleteSplitdetails(int id)
        {
           
           
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "in_ID", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(id) });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            DParam = LstParam.ToArray();
                int Result = DataAccess.ExecuteNonQuery("DeleteOBLSplit", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                try
                {
                    if (Result == 1)
                    {
                        _DBResponse.Data = null;
                        _DBResponse.Message = "Record Deleted Successfully";
                        _DBResponse.Status = 1;
                    }
                    else if (Result == 2)
                    {
                        _DBResponse.Data = null;
                        _DBResponse.Message = "Can not Delete is already Invoice Done!";
                        _DBResponse.Status = 2;
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

        #region BEO Details

        public void ListOfICEGateBEONo(string BEO, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BEO", MySqlDbType = MySqlDbType.VarChar, Value = BEO });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
         
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBEONoForBEOQuery", CommandType.StoredProcedure, Dparam);
            IList<OBLNoForPage> LstObl = new List<OBLNoForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstObl.Add(new OBLNoForPage
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_BEODate", MySqlDbType = MySqlDbType.Date, Value =Convert.ToDateTime(BEODate) });

            IDataParameter[] Dparam = lstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("GetBOENoDetails", CommandType.StoredProcedure, Dparam);
            BEODetails objBEODetails = new BEODetails();
              _DBResponse = new DatabaseResponse();
            try
            {
                Status = 1;
                objBEODetails.BeoNoDetails.BOE_NO = Convert.ToString(Result.Tables[0].Rows[0]["BOE_NO"]);
                objBEODetails.BeoNoDetails.BOE_DATE = Convert.ToString(Result.Tables[0].Rows[0]["BOE_DATE"]);

                objBEODetails.BeoNoDetails.IMP_NAME = Convert.ToString(Result.Tables[0].Rows[0]["IMP_NAME"]);
                objBEODetails.BeoNoDetails.IEC_CD = Convert.ToString(Result.Tables[0].Rows[0]["IEC_CD"]);

                objBEODetails.BeoNoDetails.CHA_CODE  = Convert.ToString(Result.Tables[0].Rows[0]["CHA_CODE"]);
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
             

                foreach(DataRow dr in Result.Tables[2].Rows)
                {
                    objBEODetails.BeoIGMDetails.Add(new BeoIGMDetails
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

        #region ContainerUpdation
        public void GetAllContainerNoForGateEntry(int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_cont", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNoForShipping", CommandType.StoredProcedure, DParam);
            List<ContainerInfoV2> LstContainerInfo = new List<ContainerInfoV2>();
            string size = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;     
                while (Result.Read())
                {
                    Status = 1;
                    LstContainerInfo.Add(new ContainerInfoV2
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        Size = Convert.ToString(Result["Size"])
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
                    _DBResponse.Data = new { LstContainerInfo, State };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetOBLContainerList(string CFSCode = "")
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.String, Value = CFSCode });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getoblcontainerlistV2", CommandType.StoredProcedure, DParam);
            List<ContainerInfoV2> LstContainerInfo = new List<ContainerInfoV2>();
            string size = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    if (CFSCode == "" || CFSCode == null)
                    {
                        LstContainerInfo.Add(new ContainerInfoV2
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                  
                        });
                    }
                    else
                    {
                        size = Convert.ToString(Result["Size"]);
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    if (CFSCode == "" || CFSCode == null)
                    {
                        _DBResponse.Data = LstContainerInfo;
                    }
                    else
                    {
                        _DBResponse.Data = size;
                    }

                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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


        //Gate Entry Shipping 
        public void ListOfShippingLineGate(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForJO", CommandType.StoredProcedure, Dparam);
            IList<ShippingLineForPage> lstShippingLine = new List<ShippingLineForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new ShippingLineForPage
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
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
                    _DBResponse.Data = new { lstShippingLine, State };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        //  Update Container 
        public void UpdateContainer(OBLEntryGateEntryV2 objOBL)
        {
            string GeneratedClientId = "";
            string Radiochecked = "";
            string CFSCode = "";
            int ShippingLineId = 0;
            string ShippingLineName = "";
            string ContainerNo = "";
            string Size = "";
            if(objOBL.ROblEntry== "OblEntrys")
            {
                Radiochecked = objOBL.ROblEntry;
                CFSCode = objOBL.CFSCode;
                ShippingLineId = objOBL.ShippingLineId;
                ShippingLineName = objOBL.ShippingLineName;
                ContainerNo = objOBL.ContainerNo;
                Size = objOBL.ContainerSize;
            }
            if(objOBL.RGateEntry== "GateEntrys")
            {
                Radiochecked = objOBL.RGateEntry;
                CFSCode = objOBL.CFSCodeGate;
                ShippingLineId = objOBL.ShippingLineIdGate;
                ShippingLineName = objOBL.ShippingLineNameGate;
                ContainerNo = objOBL.ContainerNoGate;
                Size = objOBL.ContainerSizeGate;
            }
            
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.String, Value = CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.String, Value = Size });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.String, Value = ContainerNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.String, Value = ShippingLineName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ShippingLineId }); 
            lstParam.Add(new MySqlParameter { ParameterName = "in_Radiochecked", MySqlDbType = MySqlDbType.String, Value = Radiochecked });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });


            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("UpdateShippingOblGateEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "Update Successfully" : "Updated UnSuccessfully";
                    _DBResponse.Status = Result;
                }
                else 
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "This Container information is already exists!";
                    _DBResponse.Status = Result;
                }
         
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }

        public void GetAllOblEntryGateEntry(int Page=0)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
                //LstParam.Add(new MySqlParameter { ParameterName = "in_uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });

                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetUpdateGateEntryOblEntry", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<OBLEntryGateEntryV2> OblEntryList = new List<OBLEntryGateEntryV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        OBLEntryGateEntryV2 objOBLEntry = new OBLEntryGateEntryV2();
                        //objOBLEntry.Id = Convert.ToInt32(dr["Id"]);
                        objOBLEntry.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                                 objOBLEntry.CFSCode = Convert.ToString(dr["CFSCode"]);
                          objOBLEntry.ShippingLineId = Convert.ToInt32(dr["ShippingLineId"]);
                        objOBLEntry.ShippingLineName = Convert.ToString(dr["ShippingLine"]);
                 
                        OblEntryList.Add(objOBLEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OblEntryList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetListOfOBLEntryByContainerNoFor(string ContainerNo)
        {
            int Status = 0;
            string[] EmailSplit = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("UpdateListOfOBLEntryGateEntryByContainerNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OBLEntryGateEntryV2> OblEntryList = new List<OBLEntryGateEntryV2>();
            ///  List<PPGEximTrader> LstEximTrader = new List<PPGEximTrader>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // EmailSplit = ((Result["Email"] == null ? "" : Result["Email"]).ToString()).Trim().Split(',');
                    // var Email = string.Join("\n ", EmailSplit);
                    OblEntryList.Add(new OBLEntryGateEntryV2
                    {
                        //Id = Convert.ToInt32(Result["Id"]),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"].ToString()),
                        ShippingLineName = Convert.ToString(Result["ShippingLine"])
          

                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OblEntryList;
                }
                else
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

        #endregion ContainerUpdation

        #region IGM Data Populate by Container
        public void GetContainerList(string Year)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Year)});
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForIGM", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Ppg_IGMData ObjIGM = new Ppg_IGMData();
            List<Ppg_IGMData> lstContainer = new List<Ppg_IGMData>();
            try
            {
                while (Result.Read())
                {
                    Status = 1; 
                    lstContainer.Add(new Ppg_IGMData
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                    });
                }



                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstContainer;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetYearList()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLast10Years", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Ppg_IGMData ObjIGM = new Ppg_IGMData();
            List<Ppg_IGMData> lstYear = new List<Ppg_IGMData>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstYear.Add(new Ppg_IGMData
                    {
                        Year = Result["Year"].ToString(),
                    });
                }



                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstYear;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetContainerCargoInfo(string Year,string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Year) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerCargoInfo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Ppg_IGMData ObjIGM = new Ppg_IGMData();
            List<Ppg_ContainerInfo> lstContainerInfo = new List<Ppg_ContainerInfo>();
            List<Ppg_CargoInfo> lstCargoInfo = new List<Ppg_CargoInfo>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstContainerInfo.Add(new Ppg_ContainerInfo
                    {
                        Size = Result["CONT_SIZE"].ToString(),
                        Status = Result["CONT_STATUS"].ToString(),
                        IGMNo = Result["IGM_NO"].ToString(),
                        IGMDate = Result["IGM_DATE"].ToString(),
                        TPNo = Result["TP_NO"].ToString(),
                        TPDate = Result["TP_DATE"].ToString(),
                        OBLNo = Result["OBL_NO"].ToString(),
                        NoOfPkg = Result["NO_PKG"].ToString(),
                        Weight = Result["GR_WT"].ToString(),
                        SealNo = Result["CONT_SEAL_NO"].ToString(),
                        ISOCode = Result["ISO_CODE"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstCargoInfo.Add(new Ppg_CargoInfo
                        {
                            OBLNo = Result["OBL_NO"].ToString(),
                            OBLDate = Result["OBL_DATE"].ToString(),
                            ImporterName = Result["IMP_NAME"].ToString(),
                            NoOfPkg = Result["NO_PKG"].ToString(),
                            PkgType = Result["PKG_TYPE"].ToString(),
                            Weight = Result["GR_WT"].ToString(),
                            Unit = Result["UNIT_OF_WEIGHT"].ToString(),
                            CargoDesc = Result["CARGO_DESC"].ToString(),
                            HOUSE_BL_NO = Result["HOUSE_BL_NO"].ToString()
                        });
                    }
                }
                ObjIGM.lstContainerInfo = lstContainerInfo;
                ObjIGM.lstCargoInfo = lstCargoInfo;

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjIGM;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region Approve Delivery Order

        public void GetDestuffEntryNoForDeliveryOrder(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Uid) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffEntryNoForApprovedDeliveryOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DestuffingEntryNoList> LstDestuffEntryNo = new List<DestuffingEntryNoList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffEntryNo.Add(new DestuffingEntryNoList
                    {
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        OBLDate = Result["OBLDate"].ToString(),
                        DestuffingId = Convert.ToInt32(Result["DestuffingId"]),
                        ImporterName = Result["ImporterName"].ToString(),
                        ImporterId = Convert.ToInt32(Result["ImporterId"]),
                        ContainerNo = Result["ContainerNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDestuffEntryNo;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetMobileNoForDeliveryOrder(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Uid) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMobileNoDeliveryOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            string MobileNo = "";
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    MobileNo = Convert.ToString(Result["MobileNo"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = MobileNo;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void AddeditApprovedDeliveryOrder(Ppg_ApproveDeliveryOrder vm)
        {
            int Status = 0;
            // string OBLDate = DateTime.ParseExact(vm.OBLDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLNo", MySqlDbType = MySqlDbType.String, Value = vm.OBLNo });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_OBLDate", MySqlDbType = MySqlDbType.String, Value = OBLDate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = vm.DestuffingId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreateBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
          
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditDeliveryOrderApprovedForExternal", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Delivery Order Approved Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update delivery application already done";
                    _DBResponse.Status = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update cargo seize already done";
                    _DBResponse.Status = Result;
                }

                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "This OBL can not be Ammendment because that OBL destuffed multiple Godown";
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


        public void GetOBLStatus(int DestuffingId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(DestuffingId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_OBLStatusDetails> lstOBLStatus = new List<Ppg_OBLStatusDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstOBLStatus.Add(new Ppg_OBLStatusDetails
                    {
                        BOLNo = Convert.ToString(Result["BOLNo"]),
                        CARGO_DESC = Convert.ToString(Result["CARGO_DESC"]),
                        CIFValue = Convert.ToDecimal(Result["CIFVal"]),
                        CONTAINER_NO = Convert.ToString(Result["CONTAINER_NO"]),
                        Duty = Convert.ToDecimal(Result["Duty"]),
                        GRWT = Convert.ToDecimal(Result["GR_WT"]),
                        IMP_NAME = Convert.ToString(Result["IMP_NAME"]),
                        NOPKG = Convert.ToInt32(Result["NO_PKG"])
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstOBLStatus;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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


        public void GetOBLListForKnowStorageCharge(int uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(uid) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLNoForKnowYourStorage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_ApproveDeliveryOrder> lstOBLStatus = new List<Ppg_ApproveDeliveryOrder>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstOBLStatus.Add(new Ppg_ApproveDeliveryOrder
                    {
                        OBLNo = Convert.ToString(Result["BOLNo"]),
                        DestuffingId = Convert.ToInt32(Result["DestuffingEntryId"]),

                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstOBLStatus;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetSBListForKnowStorageCharge(int uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(uid) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSBNoForKnowYourStorage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_ApproveDeliveryOrder> lstOBLStatus = new List<Ppg_ApproveDeliveryOrder>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstOBLStatus.Add(new Ppg_ApproveDeliveryOrder
                    {
                        OBLNo = Convert.ToString(Result["ShippingBillNo"]),
                        DestuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstOBLStatus;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region Know Your Storage Charge

        public void GetKnowStorageCharge(string OBL, string SBNo, string Date, int RefId)
        {
            int Status = 0;
            string DeliveryDate = DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBL", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(OBL) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SB", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(SBNo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DeliveryDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RefId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(RefId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetKnowYourStorageCharge", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            decimal TotalStorageCharge = 0M;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    TotalStorageCharge = Convert.ToDecimal(Result["StorageCharge"]);


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = TotalStorageCharge;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region Self Assessment

        public void GetDestuffEntryNoForSelfAssessment(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Uid) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffEntryNoForSelfAssessment", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DestuffingEntryNoList> LstDestuffEntryNo = new List<DestuffingEntryNoList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffEntryNo.Add(new DestuffingEntryNoList
                    {
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        OBLDate = Result["OBLDate"].ToString(),
                        DestuffingId = Convert.ToInt32(Result["DestuffingId"]),
                        ImporterName = Result["ImporterName"].ToString(),
                        ImporterId = Convert.ToInt32(Result["ImporterId"]),
                        ContainerNo = Result["ContainerNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDestuffEntryNo;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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