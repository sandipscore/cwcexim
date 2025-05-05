using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using CwcExim.Models;
using System.Data;
using CwcExim.Areas.Import.Models;
using System.Globalization;
using Newtonsoft.Json;
using CwcExim.Areas.Master.Models;
using EinvoiceLibrary;

namespace CwcExim.Repositories
{
    public class AMD_ImportRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal void AmdaddTest()
        {
            throw new NotImplementedException();
        }

        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }


        public void GetAllCommodity()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCommodity", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Export.Models.Commodity> LstCommodity = new List<Areas.Export.Models.Commodity>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCommodity.Add(new Areas.Export.Models.Commodity
                    {
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        CommodityName = Result["CommodityName"].ToString()
                        // CommodityAlias = (Result["CommodityAlias"] == null ? "" : Result["CommodityAlias"]).ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCommodity;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch
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

        #region Train Summary

        public void GetOBLContainerListOrSize(string CFSCode = "")
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.String, Value = CFSCode });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLContainerList", CommandType.StoredProcedure, DParam);
            List<CwcExim.Areas.Report.Models.ContainerInfo> LstContainerInfo = new List<CwcExim.Areas.Report.Models.ContainerInfo>();
            string size = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    if (CFSCode == "" || CFSCode == null)
                    {
                        LstContainerInfo.Add(new CwcExim.Areas.Report.Models.ContainerInfo
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            MovementType = Convert.ToString(Result["MovementType"])
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



        public void AddUpdateTrainSummaryUpload(TrainSummaryUpload Obj, string TrainSummaryUploadXML = "")
        {
            try
            {
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainSummaryId", MySqlDbType = MySqlDbType.Int32, Value = Obj.TrainSummaryUploadId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.TrainNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Obj.TrainDate) });
                LstParam.Add(new MySqlParameter { ParameterName = "In_PortId", MySqlDbType = MySqlDbType.Int32, Value = Obj.PortId });
                LstParam.Add(new MySqlParameter { ParameterName = "TrainSummaryUploadXML", MySqlDbType = MySqlDbType.String, Value = TrainSummaryUploadXML });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("AddUpdateTrainSummaryUpload", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                _DBResponse.Status = 1;
                _DBResponse.Message = "";
                _DBResponse.Data = Result;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        public void CheckTrainSummaryUpload(string TrainNo, string TrainSummaryUploadXML)
        {
            DataSet Result = new DataSet();
            try
            {

                int RetValue = 0;
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = TrainNo }); ;
                LstParam.Add(new MySqlParameter { ParameterName = "TrainSummaryUploadXML", MySqlDbType = MySqlDbType.String, Value = TrainSummaryUploadXML });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = RetValue });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("CheckTrainSummaryUpload", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                RetValue = Convert.ToInt32(DParam.Where(x => x.ParameterName == "RetValue").Select(x => x.Value).FirstOrDefault());

                List<TrainSummaryUpload> TrainSummaryUploadList = new List<TrainSummaryUpload>();
                foreach (DataRow dr in Result.Tables[0].Rows)
                {

                    TrainSummaryUpload objTrainSummaryUpload = new TrainSummaryUpload();
                    objTrainSummaryUpload.SrNo = TrainSummaryUploadList.Count + 1;
                    objTrainSummaryUpload.Wagon_No = Convert.ToString(dr["Wagon_No"]);
                    objTrainSummaryUpload.Container_No = Convert.ToString(dr["Container_No"]);
                    objTrainSummaryUpload.CT_Size = Convert.ToString(dr["CT_Size"]);
                    objTrainSummaryUpload.Line_Seal_No = Convert.ToString(dr["Line_Seal_No"]);
                    objTrainSummaryUpload.Cont_Commodity = Convert.ToString(dr["Cont_Commodity"]);
                    objTrainSummaryUpload.S_Line = Convert.ToString(dr["S_Line"]);
                    objTrainSummaryUpload.Ct_Tare = Convert.ToDecimal(dr["Ct_Tare"]);
                    objTrainSummaryUpload.Cargo_Wt = Convert.ToDecimal(dr["Cargo_Wt"]);
                    objTrainSummaryUpload.Gross_Wt = Convert.ToDecimal(dr["Gross_Wt"]);
                    objTrainSummaryUpload.Ct_Status = Convert.ToString(dr["Ct_Status"]);
                    objTrainSummaryUpload.Destination = Convert.ToString(dr["Destination"]);
                    objTrainSummaryUpload.Smtp_No = Convert.ToString(dr["Smtp_No"]);
                    objTrainSummaryUpload.Received_Date = Convert.ToString(dr["Received_Date"]);
                    objTrainSummaryUpload.Genhaz = Convert.ToString(dr["Genhaz"]);
                    objTrainSummaryUpload.Status = Convert.ToInt32(dr["Status"]);

                    if (objTrainSummaryUpload.Status == 0)
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
                    }


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
            finally {
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

                OBLEntry objOBLEntry = new OBLEntry();

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
                        OblEntryDetails objOBLEntryDetails = new OblEntryDetails();
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

        public void GetAllOBLPort()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllOBLPort", CommandType.StoredProcedure);
            List<Port> LstPort = new List<Port>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new Port
                    {
                        PortName = Result["PortName"].ToString(),
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

        public void AddEditOBLEntry(OBLEntry objOBL, string XmlText)
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
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditOBLEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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

        public void GetAllTrainSummary()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetTrainSummaryList", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<TrainSummaryUpload> TrainSummaryUploadList = new List<TrainSummaryUpload>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        TrainSummaryUpload objTrainSummaryUpload = new TrainSummaryUpload();
                        objTrainSummaryUpload.TrainNo = Convert.ToString(dr["TrainNo"]);
                        objTrainSummaryUpload.TrainDate = Convert.ToString(dr["TrainDate"]);
                        objTrainSummaryUpload.PortId = Convert.ToInt32(dr["PortId"]);
                        objTrainSummaryUpload.PortName = Convert.ToString(dr["PortName"]);
                        //objTrainSummaryUpload.UploadDate = Convert.ToString(dr["UploadDate"].ToString());

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

        public void GetOBLContainer(String OBLNo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLNo", MySqlDbType = MySqlDbType.VarChar, Value = OBLNo });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLContainer", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                VRN_CustomAppraisement objOBLEntry = new VRN_CustomAppraisement();
                //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        VRN_CustomAppraisementOBLCont objOBLCont = new VRN_CustomAppraisementOBLCont();
                        objOBLCont.CFSCode = Convert.ToString(dr["CFSCode"]);
                        objOBLCont.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objOBLEntry.CustomAppraisementCont.Add(objOBLCont);
                    }
                }
                if (Status == 1)
                {

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objOBLEntry.CustomAppraisementCont;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetBOEDetail(String BOENo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "in_BOE_NO", MySqlDbType = MySqlDbType.VarChar, Value = BOENo });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBOEDetail", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                VRN_CustomAppraisementBOECont objOBLCont = new VRN_CustomAppraisementBOECont();
                if (Result != null && Result.Tables.Count > 0)
                {

                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        objOBLCont.BOENo = Convert.ToString(dr["BOE_NO"]);
                        objOBLCont.BOEDate = Convert.ToString(dr["BOE_Date"]);
                        objOBLCont.CIFValue = Convert.ToDecimal(dr["CIF_VALUE"]);
                        objOBLCont.Duty = Convert.ToDecimal(dr["DUTY"]);
                    }
                }
                if (Status == 1)
                {

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objOBLCont;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetContainerOBL(String CFSCode)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetContainerOBL", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                VRN_CustomAppraisement objOBLEntry = new VRN_CustomAppraisement();

                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        VRN_CustomAppraisementOBLCont objOBLCont = new VRN_CustomAppraisementOBLCont();
                        objOBLCont.OBLNo = Convert.ToString(dr["OBLNo"]);
                        objOBLCont.OBLDate = Convert.ToString(dr["OBLDate"]);
                        objOBLCont.NoOfPackages = Convert.ToInt32(dr["NO_PKG"]);
                        objOBLCont.GrossWeight = Convert.ToDecimal(dr["GR_WT"]);
                        objOBLCont.CIFValue = Convert.ToDecimal(dr["CIFValue"]);
                        objOBLEntry.CustomAppraisementCont.Add(objOBLCont);
                    }
                }
                if (Status == 1)
                {

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objOBLEntry.CustomAppraisementCont;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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


        public void GetTrainSummaryDetails(int TrainSummaryUploadId)
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
                Result = DataAccess.ExecuteDataSet("GetTrainSummaryDetails", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                List<TrainSummaryUpload> TrainSummaryUploadList = new List<TrainSummaryUpload>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        TrainSummaryUpload objTrainSummaryUpload = new TrainSummaryUpload();
                        objTrainSummaryUpload.SrNo = TrainSummaryUploadList.Count + 1;
                        objTrainSummaryUpload.Container_No = Convert.ToString(dr["Container_No"]);
                        objTrainSummaryUpload.CT_Size = Convert.ToString(dr["CT_Size"]);
                        objTrainSummaryUpload.Line_Seal_No = Convert.ToString(dr["Line_Seal_No"]);
                        objTrainSummaryUpload.Cont_Commodity = Convert.ToString(dr["Cont_Commodity"]);
                        objTrainSummaryUpload.S_Line = Convert.ToString(dr["S_Line"]);

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

        public void ListOfTrainSummary()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetTrainSummaryList", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<TrainSummaryUpload> TrainSummaryUploadList = new List<TrainSummaryUpload>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        TrainSummaryUpload objTrainSummaryUpload = new TrainSummaryUpload();
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

        public void GetAllOblEntry(int Page)
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
                Result = DataAccess.ExecuteDataSet("GetAllOblEntryForPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<OBLEntry> OblEntryList = new List<OBLEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        OBLEntry objOBLEntry = new OBLEntry();
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

        public void GetOblEntryDetailsByOblEntryId(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetOblEntryDetailsByOblEntryId", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            OBLEntry objOBLEntry = new OBLEntry();
            IList<OblEntryDetails> lstOBLEntryDetails = new List<OblEntryDetails>();
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
                        lstOBLEntryDetails.Add(new OblEntryDetails
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

        public void DeleteOBLEntry(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("DeleteOBLEntry", CommandType.StoredProcedure, DParam);
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
            CwcExim.Areas.Import.Models.PPGSealCuttingDateForReport objseal = new CwcExim.Areas.Import.Models.PPGSealCuttingDateForReport();
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
        public void AddEditSealCutting(VRN_SealCutting Obj, string ChargesBreakupXML, int BranchId, int Uid, string OBLXML)
        {
            try
            {
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

                LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLXML", MySqlDbType = MySqlDbType.Text, Value = OBLXML });


                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("AddEditSealCutting", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Seal Cutting Save Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Seal Cutting Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Update as it is used in another page";
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerDetailForSealCutting", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<VRN_SealCutting> lstContainerList = new List<VRN_SealCutting>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstContainerList.Add(new VRN_SealCutting
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
                        CHAShippingLineId = Convert.ToInt32(Result["CHAShippingLineId"]),
                        CHAShippingLine = Convert.ToString(Result["CHAShippingLine"]),
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

        public void ListOfBL(int OBLId, int impobldtlId, string OBLFCLLCL, string CFSCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBLId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = OBLId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_impobldtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = impobldtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBLFCLLCL", MySqlDbType = MySqlDbType.VarChar, Value = OBLFCLLCL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBLDetailForSealCutting", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.SealCutting> lstSealCuttingList = new List<Areas.Import.Models.SealCutting>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstSealCuttingList.Add(new Areas.Import.Models.SealCutting
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

        public void ListOfCHAShippingLine()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCHAShippingLineForSealCutting", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.SealCuttingCHA> lstCHAList = new List<Areas.Import.Models.SealCuttingCHA>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCHAList.Add(new Areas.Import.Models.SealCuttingCHA
                    {
                        CHAShippingLineId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAShippingLine = Convert.ToString(Result["EximTraderName"]),
                        FolioNo = Convert.ToString(Result["FolioNo"]),
                        Balance = Convert.ToDecimal(Result["Balance"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCHAList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        internal void GetInvoiceDtlForSealCutting(string trancastionDate, string gateInDate, string containerNo, string size, int cHAShippingLineId, string CFSCode, int oBLType, int CargoType)
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

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateGroundRent", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                SealCutting ObjSC = new SealCutting();
                while (result.Read())
                {
                    Status = 1;
                    ObjSC.Balance = Convert.ToDecimal(result["Balance"]);
                    ObjSC.IGSTPer = Convert.ToDecimal(result["IGSTPer"].ToString());
                    ObjSC.CGSTPer = Convert.ToDecimal(result["CGSTPer"].ToString());
                    ObjSC.SGSTPer = Convert.ToDecimal(result["SGSTPer"]);
                    ObjSC.GroundRent = Convert.ToDecimal(result["GroundRent"]);
                    ObjSC.CGST = Convert.ToDecimal(result["CGST"]);
                    ObjSC.SGST = Convert.ToDecimal(result["SGST"]);
                    ObjSC.IGST = Convert.ToDecimal(result["IGST"]);
                    ObjSC.TotalAmount = Convert.ToDecimal(result["TotalAmount"]);
                }



                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjSC.lstObldt.Add(new SealCutting
                        {
                            Balance = Convert.ToDecimal(result["Balance"])
                        });
                    }
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {

                        ObjSC.lstPostPaymentChrgBreakup.Add(new ppgSealPostPaymentChargebreakupdate
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
            finally {
                result.Close();
                result.Dispose();

            }
        }

        public void GetListOfSealCuttingDetails(int Page)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet ds = DataAccess.ExecuteDataSet("GetListOfSealCuttingForPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    List<VRN_SealCutting> lstSealCutting = new List<VRN_SealCutting>();
                    lstSealCutting = (from DataRow dr in ds.Tables[0].Rows
                                      select new VRN_SealCutting()
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
                                          //InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                                          //InvoiceNo = dr["InvoiceNo"].ToString()
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
            finally {
                ds.Dispose();
            }
        }

        public void GetSealCuttingById(int SealCuttingId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_SealCuttingId", MySqlDbType = MySqlDbType.Int32, Value = SealCuttingId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("GetSealCuttingById", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            int ctype = 2;
            try
            {
                VRN_SealCutting ObjSC = new VRN_SealCutting();
                while (result.Read())
                {
                    Status = 1;
                    ObjSC.SealCuttingId = Convert.ToInt32(result["SealCuttingId"]);
                    ObjSC.SealCuttingNo = Convert.ToString(result["SealCuttingNo"]);
                    ObjSC.TransactionDate = Convert.ToString(result["TranscationDate"].ToString());
                    ObjSC.BLNo = result["BLNo"].ToString();
                    ObjSC.BLDate = Convert.ToString(result["BLDate"].ToString());
                    ObjSC.ContainerNo = result["ContainerNo"].ToString();
                    ObjSC.ContainerId = result["ContainerId"] == DBNull.Value ? 0 : Convert.ToInt32(result["ContainerId"].ToString());
                    ObjSC.size = Convert.ToString(result["size"].ToString());
                    ObjSC.GateInDate = Convert.ToString(result["GateInDate"].ToString());
                    ObjSC.CFSCode = result["CFSCode"].ToString();
                    ObjSC.GodownNo = result["GodownNo"].ToString();
                    ObjSC.GodownId = Convert.ToInt32(result["GodownId"]);
                    ObjSC.CHAShippingLineId = Convert.ToInt32(result["CHAShippingLineId"].ToString());
                    ObjSC.CHAShippingLine = result["CHAShippingLine"].ToString();
                    ObjSC.CustomSealNo = Convert.ToString(result["CustomSealNo"]);
                    ObjSC.OBLType = Convert.ToString(result["OBLType"]);
                    ObjSC.DisplayOBLType = Convert.ToString(result["DisplayOBLType"]);
                    ObjSC.LCLFCL = Convert.ToString(result["LCLFCL"]);

                }
                //IList<Areas.Import.Models.SealCutting> lstOblList = new List<Areas.Import.Models.SealCutting>();
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        Status = 1;

                        ObjSC.lstObldt.Add(new VRN_SealCutting
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
            finally {
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
            int Result = DataAccess.ExecuteNonQuery("DeleteSealCutting", CommandType.StoredProcedure, DParam);
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
        #endregion


        #region Job Order
        public void GetAllTrainNo()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetTrainNo", CommandType.StoredProcedure, dpram);
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

        public void GetImportJODetailsFrPrint(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetDetForPrntimpjo", CommandType.StoredProcedure, dpram);
            PPGPrintJOModel objMdl = new PPGPrintJOModel();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objMdl.ContainerType = result["ContainerType"].ToString();
                    objMdl.JobOrderNo = result["JobOrderNo"].ToString();
                    objMdl.JobOrderDate = result["JobOrderDate"].ToString();
                    objMdl.ShippingLineName = result["ShippingLineName"].ToString();
                    objMdl.FromLocation = result["FromLocation"].ToString();
                    objMdl.ToLocation = result["ToLocation"].ToString();
                    objMdl.TrainNo = result["TrainNo"].ToString();
                    objMdl.TrainDate = result["TrainDate"].ToString();

                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objMdl.lstDet.Add(new PPGPrintJOModelDet
                        {
                            ContainerNo = result["ContainerNo"].ToString(),
                            ContainerSize = result["ContainerSize"].ToString(),
                            Sline = result["ShippingLineName"].ToString(),
                            CargoType = result["CargoType"].ToString(),
                            ContainerLoadType = result["ContainerLoadType"].ToString(),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objMdl;
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
        public void GetTrainDtl(int TrainSumId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_TrainSumId", MySqlDbType = MySqlDbType.Int32, Value = TrainSumId });
            //   lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetTrainDet", CommandType.StoredProcedure, dpram);
            FormOneForImpJO objFormone = new FormOneForImpJO();
            IList<PPG_TrainDtl> lstDtl = new List<PPG_TrainDtl>();
            // IList<Areas.Import.Models.Yard> lstYard = new List<Areas.Import.Models.Yard>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    // objFormone.FormOneNo = result["FormOneNo"].ToString();
                    lstDtl.Add(new PPG_TrainDtl
                    {
                        TrainSummarySerial = Convert.ToInt32(result["TrainSummarySerial"]),
                        TrainSummaryID = Convert.ToInt32(result["TrainSummaryID"]),
                        TrainNo = result["TrainNo"].ToString(),
                        TrainDate = Convert.ToDateTime(result["TrainDate"]).ToString("dd/MM/yyyy"),
                        PortId = (result["PortId"]) == null ? 0 : Convert.ToInt32(result["PortId"]),
                        Wagon_No = (result["Wagon_No"] == null ? "" : result["Wagon_No"]).ToString(),
                        Container_No = (result["Container_No"] == null ? "" : result["Container_No"]).ToString(),
                        CT_Size = (result["CT_Size"] == null ? "" : result["CT_Size"]).ToString(),
                        Line_Seal_No = (result["Line_Seal_No"] == null ? "" : result["Line_Seal_No"]).ToString(),
                        Cont_Commodity = (result["Cont_Commodity"] == null ? "" : result["Cont_Commodity"]).ToString(),
                        S_Line = (result["S_Line"] == null ? "" : result["S_Line"]).ToString(),
                        Ct_Tare = (result["Ct_Tare"] == null ? 0 : Convert.ToDecimal(result["Ct_Tare"])),
                        Cargo_Wt = (result["Cargo_Wt"] == null ? 0 : Convert.ToDecimal(result["Cargo_Wt"])),
                        Gross_Wt = (result["Gross_Wt"] == null ? 0 : Convert.ToDecimal(result["Gross_Wt"])),
                        Ct_Status = (result["Ct_Status"] == null ? "" : result["Ct_Status"]).ToString(),
                        Destination = (result["Destination"] == null ? "" : result["Destination"]).ToString(),
                        Smtp_No = (result["Smtp_No"] == null ? "" : result["Smtp_No"]).ToString(),
                        Received_Date = (result["Received_Date"] == null ? "" : result["Received_Date"]).ToString(),
                        Genhaz = (result["Genhaz"] == null ? "" : result["Genhaz"]).ToString(),
                        TransportName = result["PortName"].ToString(),

                        ShippingLineName = result["S_Linee"] == DBNull.Value ? "" : result["S_Linee"].ToString(),
                        ShippingLineId = result["S_LineId"] == DBNull.Value ? 0 : Convert.ToInt32(result["S_LineId"].ToString())

                    });
                }
                if (Status == 1)
                {

                    _DBResponse.Data = lstDtl;
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
        public void GetTrainDetailsOnEditMode(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfImpJobOrd", CommandType.StoredProcedure, dpram);
            PPG_JobOrder objImpJO = new PPG_JobOrder();
            IList<PPG_ImportJobOrderDtl> lstDtl = new List<PPG_ImportJobOrderDtl>();
            IList<PPG_ImportJobDel> lstdel = new List<PPG_ImportJobDel>();
            // IList<Areas.Import.Models.Yard> lstYard = new List<Areas.Import.Models.Yard>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;

            lstdel.Add(new PPG_ImportJobDel
            {
                JobOrderDtlId = 0
            });

            objImpJO.deleteXML = Newtonsoft.Json.JsonConvert.SerializeObject(lstdel);
            try
            {
                while (result.Read())
                {
                    Status = 1;

                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        lstDtl.Add(new PPG_ImportJobOrderDtl
                        {
                            JobOrderDtlId = Convert.ToInt32(result["JobOrderDtlId"]),
                            JobOrderId = Convert.ToInt32(result["JobOrderId"]),
                            Container_No = result["ContainerNo"].ToString(),
                            CT_Size = result["ContainerSize"].ToString(),
                            Line_Seal_No = result["Line_Seal_No"].ToString(),
                            Cont_Commodity = result["Cont_Commodity"].ToString(),
                            PortId = Convert.ToInt32(result["PortId"]),
                            CustomSealNo = result["CustomSealNo"].ToString(),
                            ShippingLineNo = result["ShippingLineNo"].ToString(),
                            ShippingLineId = Convert.ToInt32(result["ShippingLine"].ToString()),
                            ShippingLineName = result["ShippingLineName"].ToString(),
                            CargoType = result["CargoType"].ToString(),
                            ContainerLoadType = result["ContainerLoadType"].ToString(),
                            NoOfPackages = Convert.ToDecimal(result["NoOfPackages"].ToString()),
                            Wagon_No = result["Wagon_No"].ToString(),
                            S_Line = result["S_Line"].ToString(),
                            TransportName = result["Transportfrom"].ToString(),
                            Ct_Tare = Convert.ToDecimal(result["Ct_Tare"].ToString()),
                            Cargo_Wt = Convert.ToDecimal(result["Cargo_Wt"].ToString()),
                            Gross_Wt = Convert.ToDecimal(result["Gross_Wt"].ToString()),
                            Ct_Status = result["Ct_Status"].ToString(),
                            Destination = result["Destination"].ToString(),
                            Smtp_No = result["Smtp_No"].ToString(),
                            TrainSummaryID = Convert.ToInt32(result["TrainSummaryID"]),
                            TrainSummarySerial = Convert.ToInt32(result["TrainSummarySerial"]),
                            Remarks = result["Remarks"].ToString(),
                            CargoDescription = result["CargoDescription"].ToString(),
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Data = lstDtl;
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

        public void AddEditImpJO(VRN_JobOrder objJO, string XML = null)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            string Status = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = objJO.ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PickUpLocation", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objJO.PickUpLocation });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TrainSummaryID", MySqlDbType = MySqlDbType.Int32, Value = objJO.TrainSummaryID });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderFor", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objJO.JobOrderFor });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objJO.JobOrderDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_transportfor", MySqlDbType = MySqlDbType.Int32, Value = objJO.TransportBy });

            lstParam.Add(new MySqlParameter { ParameterName = "in_Train_no", MySqlDbType = MySqlDbType.VarChar, Value = objJO.TrainNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Train_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objJO.TrainDate) });
            //  lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = objJO.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.LongText, Value = XML });
            //  lstParam.Add(new MySqlParameter { ParameterName = "in_LctnXML", MySqlDbType = MySqlDbType.Text, Value = LocationXML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditJobOrder", CommandType.StoredProcedure, dpram, out Status, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Import Job Order Saved Successfully" : "Import Job Order Saved Successfully";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
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


        public void GetAllImpJO(int Page)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfImpJobOrdForPage", CommandType.StoredProcedure, dpram);
            IList<PPG_ImportJobOrderList> lstImpJO = new List<PPG_ImportJobOrderList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImpJO.Add(new PPG_ImportJobOrderList
                    {
                        ImpJobOrderId = Convert.ToInt32(result["ImpJobOrderId"]),
                        JobOrderNo = result["JobOrderNo"].ToString(),
                        JobOrderDate = result["JobOrderDate"].ToString(),
                        TrainNo = result["TrainNo"].ToString(),
                        TrainDate = result["TrainDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstImpJO;
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


        public void GetAllImpJO(string ContainerNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();

            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo.TrimStart() });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("SearchListOfJobOrder", CommandType.StoredProcedure, dpram);
            IList<PPG_ImportJobOrderList> lstImpJO = new List<PPG_ImportJobOrderList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImpJO.Add(new PPG_ImportJobOrderList
                    {
                        ImpJobOrderId = Convert.ToInt32(result["ImpJobOrderId"]),
                        JobOrderNo = result["JobOrderNo"].ToString(),
                        JobOrderDate = result["JobOrderDate"].ToString(),
                        TrainNo = result["TrainNo"].ToString(),
                        TrainDate = result["TrainDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstImpJO;
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
        public void GetImpJODetails(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfImpJobOrd", CommandType.StoredProcedure, dpram);
            PPG_JobOrder objImpJO = new PPG_JobOrder();
            IList<PPG_ImportJobOrderDtl> lstDtl = new List<PPG_ImportJobOrderDtl>();
            IList<PPG_ImportJobDel> lstdel = new List<PPG_ImportJobDel>();
            // IList<Areas.Import.Models.Yard> lstYard = new List<Areas.Import.Models.Yard>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;

            lstdel.Add(new PPG_ImportJobDel
            {
                JobOrderDtlId = 0
            });

            objImpJO.deleteXML = Newtonsoft.Json.JsonConvert.SerializeObject(lstdel);
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objImpJO.PickUpLocation = result["PickUpLocation"].ToString();
                    objImpJO.ImpJobOrderId = Convert.ToInt32(result["ImpJobOrderId"]);
                    objImpJO.JobOrderNo = result["JobOrderNo"].ToString();
                    objImpJO.JobOrderDate = Convert.ToDateTime(result["JobOrderDate"]);
                    objImpJO.TrainNo = result["TrainNo"].ToString();
                    objImpJO.TrainDate = Convert.ToDateTime(result["TrainDate"]);
                    objImpJO.TrainSummaryID = result["TrainSummaryId"].ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        lstDtl.Add(new PPG_ImportJobOrderDtl
                        {
                            JobOrderDtlId = Convert.ToInt32(result["JobOrderDtlId"]),
                            JobOrderId = Convert.ToInt32(result["JobOrderId"]),
                            Container_No = result["ContainerNo"].ToString(),
                            CT_Size = result["ContainerSize"].ToString(),
                            Line_Seal_No = result["Line_Seal_No"].ToString(),
                            Cont_Commodity = result["Cont_Commodity"].ToString(),
                            PortId = Convert.ToInt32(result["PortId"]),
                            CustomSealNo = result["CustomSealNo"].ToString(),
                            ShippingLineNo = result["ShippingLineNo"].ToString(),
                            ShippingLineId = Convert.ToInt32(result["ShippingLine"].ToString()),
                            ShippingLineName = result["ShippingLineName"].ToString(),
                            CargoType = result["CargoType"].ToString(),
                            ContainerLoadType = result["ContainerLoadType"].ToString(),
                            NoOfPackages = Convert.ToDecimal(result["NoOfPackages"].ToString()),
                            Wagon_No = result["Wagon_No"].ToString(),
                            S_Line = result["S_Line"].ToString(),
                            TransportForm = result["Transportfrom"].ToString(),
                            Ct_Tare = Convert.ToDecimal(result["Ct_Tare"].ToString()),
                            Cargo_Wt = Convert.ToDecimal(result["Cargo_Wt"].ToString()),
                            Gross_Wt = Convert.ToDecimal(result["Gross_Wt"].ToString()),
                            Ct_Status = result["Ct_Status"].ToString(),
                            Destination = result["Destination"].ToString(),
                            Smtp_No = result["Smtp_No"].ToString(),
                            TrainSummaryID = Convert.ToInt32(result["TrainSummaryID"]),
                            TrainSummarySerial = Convert.ToInt32(result["TrainSummarySerial"]),
                            Remarks = result["Remarks"].ToString(),
                            CargoDescription = result["CargoDescription"].ToString(),
                        });
                    }
                }
                if (lstDtl.Count > 0)
                {
                    objImpJO.StringifyXML = Newtonsoft.Json.JsonConvert.SerializeObject(lstDtl);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objImpJO;
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


        public void DeleteImpJO(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("DeleteImpJO", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Import Job Order Deleted Successfully";
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete As It Is Used In Another Form";
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
        }


        public void GetAllPickupLocation()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPickupLocation", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPGPickupModel> LstPickUp = new List<PPGPickupModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPickUp.Add(new PPGPickupModel
                    {
                        pickup_location = Result["PickUpLctn"].ToString(),
                        alias = (Result["LctnAlias"] == null ? "" : Result["LctnAlias"]).ToString(),
                        id = Convert.ToInt32(Result["Id"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPickUp;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
        public void GetAllPortForJobOrderTrasport()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<TransformList> LstPort = new List<TransformList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new TransformList
                    {
                        PortName = Result["PortName"].ToString(),
                        PortId = Convert.ToInt32(Result["PortId"])
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetYardWiseLocation(int YardId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_YardId", MySqlDbType = MySqlDbType.Int32, Value = YardId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetYardWiseLocation", CommandType.StoredProcedure, dpram);
            IList<Areas.Import.Models.YardWiseLocation> lstDtl = new List<Areas.Import.Models.YardWiseLocation>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstDtl.Add(new Areas.Import.Models.YardWiseLocation
                    {
                        LocationId = Convert.ToInt32(result["LocationId"]),
                        Location = result["Location"].ToString(),
                        //IsOccupied = Convert.ToBoolean(result["IsOccupied"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstDtl;
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



        #region Tally Sheet
        public void AddEditTallySheet(VRN_TallySheet Obj, string XML, int BranchId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TallySheetId", MySqlDbType = MySqlDbType.Int32, Value = Obj.TallySheetId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TallySheetDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(Obj.TallySheetDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealCuttingId", MySqlDbType = MySqlDbType.Int32, Value = Obj.SealCuttingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = Obj.ContainerId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Obj.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Obj.ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = Obj.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = Obj.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownNo", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = Obj.GodownName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateInNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Obj.GateInNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = XML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditTallySheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Tally Sheet Saved Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Tally Sheet Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Update as it is used in another page";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Data already exists";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        public void ListOfTallySheet(int BranchId, int Uid, int Page)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TallySheetId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet ds = DataAccess.ExecuteDataSet("ListOfTallySheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    List<TallySheetList> lstTally = new List<TallySheetList>();
                    lstTally = (from DataRow dr in ds.Tables[0].Rows
                                select new TallySheetList()
                                {
                                    TellySheetNo = dr["TallySheetNo"].ToString(),
                                    TellySheetDate = dr["TallySheetDate"].ToString(),
                                    TallySheetId = Convert.ToInt32(dr["TallySheetId"]),
                                    CFSCode = dr["CFSCode"].ToString(),
                                    ContainerNo = dr["ContainerNo"].ToString(),
                                    GodownName = dr["GodownNo"].ToString(),
                                    GateInNo = dr["GateInNo"].ToString()
                                }).ToList();
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Successful";
                    _DBResponse.Data = lstTally;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No data";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void TallySheetSearchByContainer(int BranchId, int Uid, string ContainerNo)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet ds = DataAccess.ExecuteDataSet("GetTallySheetSearchByContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    List<TallySheetList> lstTally = new List<TallySheetList>();
                    lstTally = (from DataRow dr in ds.Tables[0].Rows
                                select new TallySheetList()
                                {
                                    TellySheetNo = dr["TallySheetNo"].ToString(),
                                    TellySheetDate = dr["TallySheetDate"].ToString(),
                                    TallySheetId = Convert.ToInt32(dr["TallySheetId"]),
                                    CFSCode = dr["CFSCode"].ToString(),
                                    ContainerNo = dr["ContainerNo"].ToString(),
                                    GodownName = dr["GodownNo"].ToString(),
                                    GateInNo = dr["GateInNo"].ToString()
                                }).ToList();
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Successful";
                    _DBResponse.Data = lstTally;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No data";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        public void GetTallySheet(int BranchId, int TallySheetId, int Uid)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TallySheetId", MySqlDbType = MySqlDbType.Int32, Value = TallySheetId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet ds = DataAccess.ExecuteDataSet("ListOfTallySheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    VRN_TallySheet objTS = new VRN_TallySheet();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        objTS.TallySheetId = Convert.ToInt32(dr["TallySheetId"]);
                        objTS.TallySheetNo = dr["TallySheetNo"].ToString();
                        objTS.TallySheetDate = dr["TallySheetDate"].ToString();
                        objTS.ContainerId = Convert.ToInt32(dr["ContainerId"]);
                        objTS.CFSCode = dr["CFSCode"].ToString();
                        objTS.ContainerNo = dr["ContainerNo"].ToString();
                        objTS.Size = dr["Size"].ToString();
                        objTS.GodownId = Convert.ToInt32(dr["GodownId"]);
                        objTS.GodownName = dr["GodownNo"].ToString();
                        objTS.GateInNo = dr["GateInNo"].ToString();
                        objTS.SealCuttingId = Convert.ToInt32(dr["SealCuttingId"]);
                        objTS.SealCuttingDt = dr["SealCuttingDt"].ToString();
                    }
                    objTS.lstDtl = (from DataRow dr in ds.Tables[1].Rows
                                    select new TallySheetDtl()
                                    {
                                        TallySheetDtlId = Convert.ToInt32(dr["TallySheetDtlId"]),
                                        OblNo = dr["OblNo"].ToString(),
                                        OblDate = dr["OblDate"].ToString(),
                                        IGMNo = dr["IGMNo"].ToString(),
                                        LineNo = dr["LineNo"].ToString(),
                                        Cargo = dr["Cargo"].ToString(),
                                        Pkg = Convert.ToInt32(dr["NoOfPkg"]),
                                        Wt = Convert.ToDecimal(dr["Weight"]),
                                        UOM = dr["UOM"].ToString(),
                                        Area = Convert.ToDecimal(dr["Area"])
                                    }).ToList();
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Successful";
                    _DBResponse.Data = objTS;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No data";
                    _DBResponse.Data = null;
                }
            }
            catch
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
        public void GetAllOblCont(int BranchId, int Uid)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealCuttingId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet ds = DataAccess.ExecuteDataSet("GetAllOblCont", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    List<ContainerList> objTS = new List<ContainerList>();
                    objTS = (from DataRow dr in ds.Tables[0].Rows
                             select new ContainerList()
                             {
                                 CONTAINERNO = dr["CONTAINERNO"].ToString(),
                                 SealCuttingId = Convert.ToInt32(dr["SealCuttingId"])
                             }).ToList();
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Successful";
                    _DBResponse.Data = objTS;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No data";
                    _DBResponse.Data = null;
                }
            }
            catch
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
        public void GetOblContDet(int BranchId, int SealCuttingId, int Uid)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealCuttingId", MySqlDbType = MySqlDbType.Int32, Value = SealCuttingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("GetAllOblCont", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0, Serial = 0;
            VRN_TallySheet objTS = new VRN_TallySheet();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objTS.SealCuttingId = Convert.ToInt32(result["SealCuttingId"]);
                    objTS.ContainerId = Convert.ToInt32(result["ContainerId"]);
                    objTS.ContainerNo = result["ContainerNo"].ToString();
                    objTS.Size = result["Size"].ToString();
                    objTS.GodownName = result["GodownNo"].ToString();
                    objTS.GodownId = Convert.ToInt32(result["GodownId"]);
                    objTS.GateInNo = result["GateInNo"].ToString();
                    objTS.SealCuttingDt = result["SealCuttingDt"].ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objTS.lstDtl.Add(new TallySheetDtl
                        {
                            Serial = Convert.ToInt32(++Serial),
                            OblNo = result["OblNo"].ToString(),
                            OblDate = result["OblDate"].ToString(),
                            IGMNo = result["IGMNo"].ToString(),
                            LineNo = result["LineNo"].ToString(),
                            Cargo = result["Cargo"].ToString(),
                            Pkg = Convert.ToInt32(result["Pkg"]),
                            Wt = Convert.ToDecimal(result["Wt"]),
                            UOM = result["UOM"].ToString(),
                            Area = Convert.ToDecimal(result["Area"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Successful";
                    _DBResponse.Data = objTS;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception Ex)
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
        public void DeleteTallySheet(int TallySheetId, int BranchId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TallySheetId", MySqlDbType = MySqlDbType.Int32, Value = TallySheetId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteTallySheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Tally Sheet Deleted Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete as it exists in another page";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        public void PrintTallySheet(int TallySheetId, int BranchId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TallySheetId", MySqlDbType = MySqlDbType.Int32, Value = TallySheetId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("TallySheetPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            TallySheetPrintHeader objTS = new TallySheetPrintHeader();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objTS.TallySheetNo = result["TallySheetNo"].ToString();
                    objTS.TallySheetDate = result["TallySheetDate"].ToString();
                    objTS.TallySheetDateTime = result["TallySheetDateTime"].ToString();
                    objTS.GodownNo = result["GodownNo"].ToString();
                    objTS.ContainerNo = result["ContainerNo"].ToString();
                    objTS.Size = result["Size"].ToString();
                    objTS.GateInDate = result["GateInDate"].ToString();
                    objTS.CustomSealNo = result["CustomSealNo"].ToString();
                    objTS.SlaSealNo = result["SlaSealNo"].ToString();
                    objTS.CFSCode = result["CFSCode"].ToString();
                    objTS.IGM_No = result["IGM_No"].ToString();
                    objTS.MovementType = result["MovementType"].ToString();
                    objTS.ShippingLine = result["ShippingLine"].ToString();
                    objTS.POL = result["POL"].ToString();
                    objTS.POD = result["POD"].ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objTS.lstDetaiils.Add(new TallySheetPrintDetails
                        {
                            SMTPNo = result["SMTPNo"].ToString(),
                            OBL_No = result["OBL_No"].ToString(),
                            Importer = result["Importer"].ToString(),
                            Cargo = result["Cargo"].ToString(),
                            Type = result["Type"].ToString(),
                            NoOfPkg = Convert.ToInt32(result["NoOfPkg"]),
                            PkgRec = Convert.ToInt32(result["PkgRec"]),
                            Weight = Convert.ToDecimal(result["Weight"]),
                            Area = Convert.ToDecimal(result["Area"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Successful";
                    _DBResponse.Data = objTS;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No data";
                    _DBResponse.Data = null;
                }
            }
            catch
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

        #region Custom Appraisement Application
        public void ListOfShippingLine()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetShippingLine", CommandType.StoredProcedure, DParam);
            IList<ShippingLine> lstShippingLine = new List<ShippingLine>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new ShippingLine
                    {
                        ShippingLineId = Convert.ToInt32(result["ShippingLineId"]),
                        ShippingLineName = result["ShippingLine"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstShippingLine;
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
        public void ListOfCHA()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure, dparam);
            IList<CHA> lstCHA = new List<CHA>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHA
                    {
                        CHAId = Convert.ToInt32(result["EximTraderId"]),
                        CHAName = result["EximTraderName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstCHA;
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
        public void GetContnrDetForCustomAppraise(string CFSCode, string LineNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_LineNo", MySqlDbType = MySqlDbType.VarChar, Value = LineNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCntrDetForImpCstmAppraise", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VRN_CustomAppraisement ObjAppraisement = new VRN_CustomAppraisement();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.ContainerNo = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString();
                    ObjAppraisement.CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString();
                    ObjAppraisement.Size = (Result["Size"] == null ? "" : Result["Size"]).ToString();
                    ObjAppraisement.ContainerLoadType = (Result["ContainerLoadType"] == null ? "" : Result["ContainerLoadType"]).ToString();
                    ObjAppraisement.LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString();
                    ObjAppraisement.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjAppraisement.Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString();
                    ObjAppraisement.CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                    ObjAppraisement.CHAName = (Result["CHA"] == null ? "" : Result["CHA"]).ToString();
                    ObjAppraisement.Vessel = (Result["VesselName"] == null ? "" : Result["VesselName"]).ToString();
                    ObjAppraisement.Voyage = (Result["VoyageNo"] == null ? "" : Result["VoyageNo"]).ToString();
                    ObjAppraisement.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjAppraisement.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjAppraisement.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjAppraisement.Rotation = (Result["RotationNo"] == null ? "" : Result["RotationNo"]).ToString();
                    ObjAppraisement.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjAppraisement.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();

                    ObjAppraisement.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);

                    ObjAppraisement.GateEntryDate = Convert.ToDateTime(Result["EntryDateTime"]).ToString("MM/dd/yyyy");


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAppraisement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GodownWiseLocation(int GodownId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGdwnWiseLctn", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.GodownWiseLctn> lstGodownlctn = new List<Areas.Import.Models.GodownWiseLctn>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGodownlctn.Add(new Areas.Import.Models.GodownWiseLctn
                    {
                        LocationId = Convert.ToInt32(Result["LocationId"]),
                        LocationName = Convert.ToString(Result["LocationName"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstGodownlctn;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetAllCustomAppraisementApp()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCustomAppraisementApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VRN_CustomAppraisement> LstAppraisement = new List<VRN_CustomAppraisement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new VRN_CustomAppraisement
                    {
                        AppraisementNo = Result["AppraisementNo"].ToString(),
                        AppraisementDate = Result["AppraisementDate"].ToString(),
                        CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstAppraisement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetAllCustomAppraisementAppSearch(string AppraisementNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementNo", MySqlDbType = MySqlDbType.VarChar, Value = AppraisementNo });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCustomAppraisementAppSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VRN_CustomAppraisement> LstAppraisement = new List<VRN_CustomAppraisement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new VRN_CustomAppraisement
                    {
                        AppraisementNo = Result["AppraisementNo"].ToString(),
                        AppraisementDate = Result["AppraisementDate"].ToString(),
                        CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstAppraisement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetCustomAppraisement(int CustomAppraisementId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCustomAppraisementApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VRN_CustomAppraisement ObjAppraisement = new VRN_CustomAppraisement();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]);
                    ObjAppraisement.AppraisementNo = Result["AppraisementNo"].ToString();
                    ObjAppraisement.AppraisementDate = Result["AppraisementDate"].ToString();
                    ObjAppraisement.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    ObjAppraisement.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjAppraisement.CHAName = Result["CHA"].ToString();
                    ObjAppraisement.ShippingLine = Result["ShippingLine"].ToString();
                    ObjAppraisement.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjAppraisement.Voyage = Result["Voyage"].ToString();
                    ObjAppraisement.Rotation = Result["Rotation"].ToString();
                    ObjAppraisement.Fob = Convert.ToDecimal(Result["Fob"]);
                    ObjAppraisement.GrossDuty = Convert.ToDecimal(Result["GrossDuty"]);
                    ObjAppraisement.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    ObjAppraisement.IsDO = Convert.ToInt32(Result["IsDO"]);
                    ObjAppraisement.IsConvert = Convert.ToBoolean(Result["IsConvertToBond"] == DBNull.Value ? 0 : Result["IsConvertToBond"]);
                    ObjAppraisement.ApplicationForApp = Convert.ToInt32(Result["Application_For"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.LstAppraisementPpg.Add(new VRN_CustomAppraisementDtl
                        {
                            CustomAppraisementDtlId = Convert.ToInt32(Result["CustomAppraisementDtlId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),

                            OBLNo = (Result["OBLNo"] == null ? "" : Result["OBLNo"]).ToString(),
                            OBLDate = (Result["OBLDate"] == null ? "" : Result["OBLDate"]).ToString(),

                            CHANameId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]),
                            Importer = Result["Importer"].ToString(),
                            CHA = Result["CHA"].ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            WithoutDOSealNo = (Result["WithoutDOSealNo"] == null ? "" : Result["WithoutDOSealNo"]).ToString(),
                            ContainerType = Convert.ToInt32(Result["ContainerType"] == DBNull.Value ? 0 : Result["ContainerType"]),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            RMSValue = Convert.ToInt32(Result["RMS"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.LstCustomAppraisementOrdDtl.Add(new VRN_CustomAppraisementOrdDtl
                        {
                            OrderId = Convert.ToInt32(Result["OrderId"]),
                            CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]),
                            IssuedBy = (Result["DoIssueBy"] == null ? "" : Result["DoIssueBy"]).ToString(),
                            DeliveredTo = (Result["CargsDelivered"] == null ? "" : Result["CargsDelivered"]).ToString(),
                            ValidType = (Result["ValidType"] == null ? "" : Result["ValidType"]).ToString(),
                            ValidDate = (Result["ValidDate"] == null ? "" : Result["ValidDate"]).ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAppraisement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void AddEditCustomAppraisement(VRN_CustomAppraisement ObjAppraisement, string AppraisementXML, string CAOrdXML)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjAppraisement.CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_application_For", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.ApplicationForApp });

            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjAppraisement.AppraisementDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Vessel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Voyage });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Rotation", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Rotation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fob", MySqlDbType = MySqlDbType.Decimal, Value = ObjAppraisement.Fob });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsBond", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjAppraisement.IsConvert) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossDuty", MySqlDbType = MySqlDbType.Decimal, Value = ObjAppraisement.GrossDuty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsDO", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.IsDO });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementXML", MySqlDbType = MySqlDbType.Text, Value = AppraisementXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CAOrdXML", MySqlDbType = MySqlDbType.Text, Value = CAOrdXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditCustomAppraisementApp", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjAppraisement.CustomAppraisementId == 0 ? "Custom Appraisement Application Details Saved Successfully" : "Custom Appraisement Application Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Update Custom Appraisement Application Details As It Already Exists In Another Page";
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
        public void DelCustomAppraisement(int CustomAppraisementId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelCustomAppraisementApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Custom Appraisement Application Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Custom Appraisement Application Details As It Exist In Another Page";
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

        public void ListOfShippingLinePartyCode(string PartyCode, int Page = 0)
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
        public void GetContnrNoForCustomAppraise()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCntrNoForImpCstmAppraise", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VRN_CustomAppraisementDtl> LstAppraisement = new List<VRN_CustomAppraisementDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new VRN_CustomAppraisementDtl
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstAppraisement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region Custom Appraisement Approval

        public void ListOfChaForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForPage", CommandType.StoredProcedure, Dparam);
            IList<CHAForPage> lstCHA = new List<CHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
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
                    _DBResponse.Data = new { lstCHA, State };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void ListOfChaForPageforSingleClick(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForSinglePage", CommandType.StoredProcedure, Dparam);
            IList<CHAForPage> lstCHA = new List<CHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
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
                    _DBResponse.Data = new { lstCHA, State };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        //Access Right information
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
        public void ListOfImporterForMerge()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            // lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfImporterMergePage", CommandType.StoredProcedure);
            IList<Importer> lstImporterName = new List<Importer>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImporterName.Add(new Importer
                    {
                        ImporterId = Convert.ToInt32(result["ImporterId"]),
                        ImporterName = result["ImporterName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstImporterName;
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



        public void ListOfChaForMergeApp(string PartyCode)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            //   lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForMergePage", CommandType.StoredProcedure, Dparam);
            IList<CHAForPage> lstCHA = new List<CHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString()
                    });
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        State = Convert.ToBoolean(Result["State"]);
                //    }
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCHA;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void ListOfChaForSingleMergeApp(string PartyCode)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            //   lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForSingleMergePage", CommandType.StoredProcedure, Dparam);
            IList<CHAForPage> lstCHA = new List<CHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCHA;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void NewCustomeAppraisement(int Skip)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Skip", MySqlDbType = MySqlDbType.Int32, Value = Skip });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfNewAppfrCustApp", CommandType.StoredProcedure, dpram);
            IList<VRN_ListOfCustAppraisementAppr> lstApproval = new List<VRN_ListOfCustAppraisementAppr>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            bool State = false;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApproval.Add(new VRN_ListOfCustAppraisementAppr
                    {
                        CstmAppraiseAppId = Convert.ToInt32(result["CustomAppraisementId"]),
                        AppraisementNo = result["AppraisementNo"].ToString(),
                        BOENo = (result["BOENo"] == null ? "" : result["BOENo"]).ToString(),
                        CHA = (result["CHA"] == null ? "" : result["CHA"]).ToString(),
                        Importer = (result["Importer"] == null ? "" : result["Importer"]).ToString(),
                    });
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        State = Convert.ToBoolean(result["STATE"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = new { lstApproval = lstApproval, State = State };
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
        public void ApprovalHoldCustomAppraisement(int Skip)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Skip", MySqlDbType = MySqlDbType.Int32, Value = Skip });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfAppHoldCustApp", CommandType.StoredProcedure, dpram);
            IList<VRN_ListOfCustAppraisementAppr> lstApproval = new List<VRN_ListOfCustAppraisementAppr>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            bool State = false;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApproval.Add(new VRN_ListOfCustAppraisementAppr
                    {
                        CstmAppraiseAppId = Convert.ToInt32(result["CustomAppraisementId"]),
                        AppraisementNo = result["AppraisementNo"].ToString(),
                        BOENo = (result["BOENo"] == null ? "" : result["BOENo"]).ToString(),
                        CHA = (result["CHA"] == null ? "" : result["CHA"]).ToString(),
                        Importer = (result["Importer"] == null ? "" : result["Importer"]).ToString(),
                    });
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        State = Convert.ToBoolean(result["STATE"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = new { lstApproval, State };
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

        public void GetCstmAppraiseApplication(int CstmAppraiseAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CstmAppraiseAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCstmAppraiseApplication", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VRN_CustomAppraiseApproval ObjAppraisement = new VRN_CustomAppraiseApproval();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]);
                    // ObjAppraisement.CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]);
                    ObjAppraisement.AppraisementNo = Result["AppraisementNo"].ToString();
                    ObjAppraisement.AppraisementDate = Result["ApplicationDate"].ToString();
                    ObjAppraisement.ShippingLine = Result["ShippingLine"].ToString();
                    ObjAppraisement.Vessel = Convert.ToString(Result["Vessel"] == null ? "" : Result["Vessel"]);
                    ObjAppraisement.Voyage = Convert.ToString(Result["Voyage"] == null ? "" : Result["Voyage"]);
                    ObjAppraisement.Rotation = Result["Rotation"].ToString();
                    ObjAppraisement.Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]);
                    ObjAppraisement.GrossDuty = Convert.ToDecimal(Result["GrossDuty"] == DBNull.Value ? 0 : Result["GrossDuty"]);

                    ObjAppraisement.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);

                    ObjAppraisement.IsApproved = Convert.ToInt32(Result["IsApproved"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.LstAppraisementDtl.Add(new VRN_CustomAppraisementDtl
                        {
                            CustomAppraisementDtlId = Convert.ToInt32(Result["CustomAppraisementDtlId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),

                            Importer = Result["Importer"].ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),

                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAppraisement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void UpdateCustomApproval(int CstmAppraiseAppId, int IsApproved, int Uid)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseAppId", MySqlDbType = MySqlDbType.Int32, Value = CstmAppraiseAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Approved", MySqlDbType = MySqlDbType.Int32, Value = IsApproved });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] dpram = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("UpdateCustomApp", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Custom Appraisement Approval Details Saved Successfully";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Cannot Update Custom Appraisement Approval Details As It Already Exist In Another Page";
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
        }
        #endregion

        #region Destuffing Entry
        public void AddEditDestuffingEntry(VRN_DestuffingEntry ObjDestuffing, string DestuffingEntryXML /*, string GodownXML, string ClearLcoationXML */, int BranchId, int Uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StartDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.StartDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.DestuffingEntryDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TallySheetId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(ObjDestuffing.TallySheetId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(ObjDestuffing.ContainerId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Rotation", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ObjDestuffing.Rotation });
            /*LstParam.Add(new MySqlParameter { ParameterName = "in_FOB", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjDestuffing.FOB) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossDuty", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjDestuffing.GrossDuty) });*/

            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDestuffing.DeliveryType) });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_DOType", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDestuffing.DOType) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDestuffing.GodownId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingXML", MySqlDbType = MySqlDbType.Text, Value = DestuffingEntryXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });

            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditImpDestuffingEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjDestuffing.DestuffingEntryId == 0 ? "Destuffing Entry Details Saved Successfully" : "Destuffing Entry Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Destuffing Entry  Details Already Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Edit Destuffing Entry Details As It Already Exists In Another Page";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Data already exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 5)
                {
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Destuffing date should be greater than or equal to tally sheet date";
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
        public void DelDestuffingEntry(int DestuffingEntryId, int BranchId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelImpDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Destuffing Entry Application Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Destuffing Entry Application Details As It Exist In Another Page";
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
        public void GetDestuffingEntry(int DestuffingEntryId, int BranchId, string Action)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Action", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = Action });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VRN_DestuffingEntry ObjDestuffing = new VRN_DestuffingEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    ObjDestuffing.StartDate = (Result["StartDate"] == null ? "" : Result["StartDate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.TallySheetId = Result["TallySheetId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["TallySheetId"]);
                    ObjDestuffing.ContainerId = Convert.ToInt32(Result["ContainerId"]);
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    //ObjDestuffing.DOType = Convert.ToInt32(Result["DOType"]);
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                    ObjDestuffing.CHA = (Result["CHA"]).ToString();
                    ObjDestuffing.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjDestuffing.TallySheetDate = Result["TallySheetDate"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.lstDtl.Add(new VRN_DestuffingEntryDtl
                        {
                            DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                            TallySheetDtlId = Result["TallySheetDtlId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["TallySheetDtlId"]),
                            BOLNo = (Result["BOLNo"] == null ? "" : Result["BOLNo"]).ToString(),
                            BOLDate = (Result["BOLDate"] == null ? "" : Result["BOLDate"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            ReceivedPackages = Convert.ToInt32(Result["ReceivedPackages"] == DBNull.Value ? 0 : Result["ReceivedPackages"]),
                            UOM = Result["UOM"].ToString(),
                            Area = Convert.ToDecimal(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            DestuffingWeight = Convert.ToDecimal(Result["DestuffWeight"] == DBNull.Value ? 0 : Result["DestuffWeight"]),
                            GodownWiseLocationIds = (Result["GodownWiseLocationIds"] == null ? "" : Result["GodownWiseLocationIds"]).ToString(),
                            GodownWiseLocationNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString(),
                            Remarks = (Result["Remarks"]).ToString(),
                            TallySheetArea = Result["AreaTallySheet"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["AreaTallySheet"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            GrossDuty = Convert.ToDecimal(Result["GrossDuty"] == DBNull.Value ? 0 : Result["GrossDuty"]),
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            OBLWiseDestuffingDate = Result["OBLWiseDestuffingDate"].ToString(),
                            IsEditable = Convert.ToInt32(Result["IsEditable"] == DBNull.Value ? 0 : Result["IsEditable"]),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            Vessel = Convert.ToString(Result["Vessel"]),
                            Voyage = Convert.ToString(Result["Voyage"]),
                            StorageType = Convert.ToInt32(Result["StorageType"])
                        });
                    }
                }
                if (Action == "Edit")
                {
                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                            ObjDestuffing.lstLocation.Add(new GodownLocation
                            {
                                LocationId = Convert.ToInt32(Result["LocationId"]),
                                LocationName = Result["LocationName"].ToString()
                            });
                        }
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDestuffing;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetAllDestuffingEntry(int Page, int UId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UId", MySqlDbType = MySqlDbType.Int32, Value = UId });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Action", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntryForPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VRN_DestuffingList> LstDestuffing = new List<VRN_DestuffingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new VRN_DestuffingList
                    {
                        DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        //Rotation = Result["Rotation"].ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                        CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDestuffing;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
            IList<Areas.Import.Models.CommodityForPage> LstCommodity = new List<Areas.Import.Models.CommodityForPage>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstCommodity.Add(new Areas.Import.Models.CommodityForPage
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
        public void GetAllDestuffingEntry(int UId, string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_UId", MySqlDbType = MySqlDbType.Int32, Value = UId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo.TrimStart() });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntrySearchByContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VRN_DestuffingList> LstDestuffing = new List<VRN_DestuffingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new VRN_DestuffingList
                    {
                        DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        //Rotation = Result["Rotation"].ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                        CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDestuffing;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetContrNoForDestuffingEntry(int BranchId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContrNoForDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<dynamic> LstDestuffing = new List<dynamic>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        TallySheetId = Convert.ToInt32(Result["TallySheetId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDestuffing;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetContrDetForDestuffingEntry(int TallySheetId, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TallySheetId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TallySheetId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContrDetForDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VRN_DestuffingEntryDtl> lstdynamic = new List<VRN_DestuffingEntryDtl>();
            List<dynamic> lstdynamicloc = new List<dynamic>();
            List<dynamic> lstdynamichdr = new List<dynamic>();
            string ShippingLine = "";
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstdynamic.Add(new VRN_DestuffingEntryDtl
                    {
                        TallySheetDtlId = Convert.ToInt32(Result["TallySheetDtlId"]),
                        BOLNo = Result["OblNo"].ToString(),
                        BOLDate = (Result["OBLDATE"] == null ? "" : Result["OblDate"]).ToString(),
                        LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                        CargoDescription = (Result["Cargo"] == null ? "" : Result["Cargo"]).ToString(),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPkg"]),
                        GrossWeight = Convert.ToDecimal(Result["Weight"]),
                        UOM = Result["UOM"].ToString(),
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        Commodity = Result["Commodity"].ToString(),
                        TallySheetArea = Convert.ToDecimal(Result["AreaTallysheet"]),
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        ContainerId = Convert.ToInt32(Result["ContainerId"]),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        ReceivedPackages = Convert.ToInt32(Result["ReceivedPackages"]),
                        DestuffingWeight = Convert.ToInt32(Result["DestuffingWeight"]),
                        OBLWiseDestuffingDate = "",
                        CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                    });

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstdynamicloc.Add(new
                        {
                            LocationId = Convert.ToInt32(Result["LocationId"]),
                            LocationName = Result["LocationName"].ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ShippingLine = Convert.ToInt32(Result["ShippingLineId"]) + "," + Result["ShippingLineName"].ToString() + "," + Result["TallySheetDate"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstdynamichdr.Add(new
                        {
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CHAName = Result["CHAName"].ToString(),
                            RotationNo = Result["RotationNo"].ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstdynamic, lstdynamicloc, ShippingLine, lstdynamichdr };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetDestuffEntryForPrint(int DestuffingEntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffEntryForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VRN_DestuffingSheet ObjDestuffing = new VRN_DestuffingSheet();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    ObjDestuffing.StartDate = (Result["StartDate"] == null ? "" : Result["StartDate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.DestuffingEntryDateTime = Result["DestuffingEntryDateTime"].ToString();
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.GateInDate = Result["GateInDate"].ToString();
                    ObjDestuffing.CustomSealNo = Result["CustomSealNo"].ToString();
                    ObjDestuffing.SlaSealNo = Result["SlaSealNo"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.IGMNo = Result["IGMNo"].ToString();
                    ObjDestuffing.MovementType = Result["MovementType"].ToString();
                    ObjDestuffing.ShippingLine = Result["ShippingLine"].ToString();
                    ObjDestuffing.POL = (Result["POL"] == null ? "" : Result["POL"]).ToString();
                    ObjDestuffing.POD = (Result["POD"] == null ? "" : Result["POD"]).ToString();
                    ObjDestuffing.GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.lstDtl.Add(new VRN_DestuffingSheetDtl
                        {
                            SMTPNo = (Result["SMTPNo"] == null ? "" : Result["SMTPNo"]).ToString(),
                            OblNo = (Result["OblNo"] == null ? "" : Result["OblNo"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            Cargo = (Result["Cargo"] == null ? "" : Result["Cargo"]).ToString(),
                            Type = (Result["Type"] == null ? "" : Result["Type"]).ToString(),
                            NoOfPkg = Convert.ToInt32(Result["NoOfPkg"] == DBNull.Value ? 0 : Result["NoOfPkg"]),
                            PkgRec = Convert.ToInt32(Result["PkgRec"] == DBNull.Value ? 0 : Result["PkgRec"]),
                            Weight = Convert.ToDecimal(Result["Weight"] == DBNull.Value ? 0 : Result["Weight"]),
                            Area = Convert.ToDecimal(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            GodownWiseLctnNames = (Result["GodownWiseLctnNames"]).ToString(),
                            Remarks = (Result["Remarks"]).ToString(),
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDestuffing;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetCIFandDutyForBOE(string BOENo, string BOEDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(BOEDate).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCIFDutyForBOE", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            dynamic obj = new { CIFValue = "", GrossDuty = "" };
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    obj = new
                    {
                        CIFValue = Result["CIF_VALUE"].ToString(),
                        GrossDuty = Result["DUTY"].ToString()
                    };
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


        #region Yard Invoice

        public void GetAppraismentRequestForPaymentSheet(int AppraisementAppId = 0, string Type = "I")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = AppraisementAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppraismentRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaySheetStuffingRequest> objPaySheetStuffing = new List<PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new PaySheetStuffingRequest()
                    {
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        StuffingReqId = Convert.ToInt32(Result["CustomAppraisementId"]),
                        DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                        StuffingReqNo = Convert.ToString(Result["AppraisementNo"]),
                        StuffingReqDate = Convert.ToString(Result["AppraisementDate"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetYardPaymentSheet(string InvoiceDate, int AppraisementId, string InvoiceType, string ContainerXML, string Movement, int InvoiceId, int OTHours, int PartyId,
            int PayeeId, int isdirect, string SEZ, int ReeferHours, string CustomExam, int Distance, int IsFranchise, int IsOnWheel, int IsReworking, int IsCargoShifting,
            int IsLiftOnOff, int IsSweeping, int IsHandling)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Value = OTHours });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_isdirect", MySqlDbType = MySqlDbType.Int32, Value = isdirect });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Movement", MySqlDbType = MySqlDbType.VarChar, Value = Movement });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = SEZ });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReeferHours", MySqlDbType = MySqlDbType.Int32, Value = ReeferHours });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomExam", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = CustomExam });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value = Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsFranchise", MySqlDbType = MySqlDbType.Int32, Value = IsFranchise });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsOnWheel", MySqlDbType = MySqlDbType.Int32, Value = IsOnWheel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsReworking", MySqlDbType = MySqlDbType.Int32, Value = IsReworking });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsCargoShifting", MySqlDbType = MySqlDbType.Int32, Value = IsCargoShifting });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsLiftOnOff", MySqlDbType = MySqlDbType.Int32, Value = IsLiftOnOff });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsSweeping", MySqlDbType = MySqlDbType.Int32, Value = IsSweeping });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsHandling", MySqlDbType = MySqlDbType.Int32, Value = IsHandling });            
            
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            VRN_InvoiceYard objInvoice = new VRN_InvoiceYard();
            IDataReader Result = DataAccess.ExecuteDataReader("GetYardPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();

                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPrePaymentCont.Add(new VRN_PreInvoiceContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            OBLNo = Result["OBLNo"].ToString(),
                            SealCutDate = Result["SealCutDate"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerPack = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ApproveOn = Result["ApproveOn"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            OperationType = Convert.ToInt32(Result["OperationType"]),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new VRN_PostPaymentContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            OBLNo = Result["OBLNo"].ToString(),
                            SealCutDate = Result["SealCutDate"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new VRN_PostPaymentChrg
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                        });
                    }


                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContWiseAmount.Add(new VRN_ContainerWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                            CustomExamCharge = Convert.ToDecimal(Result["CustomExamCharge"]),
                            ContainerId = 0,
                            InvoiceId = 0,
                            LineNo = ""
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new VRN_OperationCFSCodeWiseAmount
                        {
                            InvoiceId = InvoiceId,
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Clause = Convert.ToString(Result["Clause"])
                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PaymentMode = Result["IN_MODE"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.NDays = Result["NDays"].ToString();
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        objInvoice.lstPostPaymentChrgBreakup.Add(new VRN_PostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            Startdate = Result["StartDate"].ToString(),
                            EndDate = Result["EndDate"].ToString(),

                        });
                    }


                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        objInvoice.IsLocalGST = Convert.ToBoolean(Result["IsLocalGST"]);
                    }


                }

                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

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


        public void AddEditInvoice(VRN_InvoiceYard ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML, string ChargesBreakupXML,
            int BranchId, int Uid, int Direct, string SEZ, string CustomExam,
           string Module, string Movement, int IsFranchise, int IsOnWheel, int IsReworking, int IsCargoShifting, int IsLiftOnOff, int IsSweeping, int IsHandling, int ReeferHours = 0, int Distance = 0, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsDirect", MySqlDbType = MySqlDbType.Int32, Value = Direct });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Movement", MySqlDbType = MySqlDbType.VarChar, Value = Movement });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReeferHours", MySqlDbType = MySqlDbType.Int32, Value = ReeferHours });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomExam", MySqlDbType = MySqlDbType.VarChar, Value = CustomExam });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value = Distance });

            LstParam.Add(new MySqlParameter { ParameterName = "in_IsFranchise", MySqlDbType = MySqlDbType.Int32, Value = IsFranchise });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsOnWheel", MySqlDbType = MySqlDbType.Int32, Value = IsOnWheel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsReworking", MySqlDbType = MySqlDbType.Int32, Value = IsReworking });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsCargoShifting", MySqlDbType = MySqlDbType.Int32, Value = IsCargoShifting });           
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsLiftOnOff", MySqlDbType = MySqlDbType.Int32, Value = IsLiftOnOff });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsSweeping", MySqlDbType = MySqlDbType.Int32, Value = IsSweeping });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsHandling", MySqlDbType = MySqlDbType.Int32, Value = IsHandling });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceYard", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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
        public void GetImpPaymentPartyForFCLPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPartyForFCLPage", CommandType.StoredProcedure, Dparam);
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

        public void ListOfYardInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofYardInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<CwcExim.Areas.Export.Models.VRN_ListOfExpInvoice> lstExpInvoice = new List<CwcExim.Areas.Export.Models.VRN_ListOfExpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new CwcExim.Areas.Export.Models.VRN_ListOfExpInvoice()
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

        public void GetContainerForPaymentSheet(int AppraisementAppId, string Type = "I")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = AppraisementAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppraismentRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaymentSheetContainer> objPaymentSheetContainer = new List<PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Selected = false,
                        ArrivalDt = Convert.ToString(Result["ArrivalDt"]),
                        IsHaz = Convert.ToString(Result["IsHaz"]),
                        Size = Convert.ToString(Result["Size"]),
                        OBLNo = Convert.ToString(Result["OBLNo"]),
                        SealCutDate = Convert.ToString(Result["SealCutDate"]),
                        NoOfPkg = Convert.ToInt32(Result["NoOfPackages"]),
                        GrWait = Convert.ToDecimal(Result["GrossWeight"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheetContainer.GroupBy(o => o.CFSCode).Select(o => o.First()).ToList();
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
                            PrdDesc = null,// Result["PrdDesc"].ToString(),
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
        public void GetIRNB2CForYard(String InvoiceNo)
        {
            VRN_IrnB2CDetails Obj = new VRN_IrnB2CDetails();

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
                    log.Info("After Calling Stored Procedure Error" + " Invoice No " + InvoiceNo + " signed Invoice: " + objOBL.SignedInvoice + " SignedQRCode " + objOBL.SignedQRCode);
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

        #region Internal Movement
        public void GetAllInternalMovement()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_Internal_Movement> LstInternalMovement = new List<PPG_Internal_Movement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new PPG_Internal_Movement
                    {
                        MovementNo = Result["MovementNo"].ToString(),
                        BOENo = Result["BOENo"].ToString(),
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


        public void GetInternalPaymentSheetInvoice(int DestuffingId, string OBLNo, String MovementDate,
            string InvoiceType, int DestLocationIdiceId, int InvoiceId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBLNo", MySqlDbType = MySqlDbType.VarChar, Value = OBLNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(MovementDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "In_DestLocationId", MySqlDbType = MySqlDbType.Int32, Value = DestLocationIdiceId });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();


            PPGInvoiceGodown objInvoice = new PPGInvoiceGodown();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInternlMovementPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["ShippingLineId"]);
                        objInvoice.CHAName = Result["ShippingLineName"].ToString();
                        objInvoice.PartyName = Result["ShippingLineName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["DestuffingId"]);
                        objInvoice.RequestNo = Result["DestuffingNo"].ToString();
                        objInvoice.RequestDate = Result["DestuffingDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();

                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPrePaymentCont.Add(new PpgPreInvoiceContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerPack = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["DelDuty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = " ",
                            CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ApproveOn = "0",
                            BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            OperationType = 0,
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new PpgPostPaymentContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["DelDuty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = " ",
                            CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContWiseAmount.Add(new PpgContainerWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                            ContainerId = 0,
                            InvoiceId = 0,
                            LineNo = ""
                        });
                    }
                }

                if (Result.NextResult())
                {
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new PpgPostPaymentChrg
                        {
                            ChargeId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"] == System.DBNull.Value ? "" : Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                            Discount = Result["Discount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Discount"]),
                            Taxable = Result["Taxable"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Result["IGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Result["IGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Result["SGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Result["SGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Result["CGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Result["CGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Result["Total"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Total"]),
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                        });
                    }


                }

                if (Result.NextResult())
                {
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new PpgOperationCFSCodeWiseAmount
                        {
                            InvoiceId = InvoiceId,
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"] == System.DBNull.Value ? "" : Result["ContainerNo"].ToString(),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    if (Result.Read())
                    {
                        objInvoice.hasSDAvailableBalance = Convert.ToInt32(Result["hasSDAvailableBalance"]);
                    }
                }

                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

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


        public void AddEditInvoiceMovement(PPG_Internal_Movement ObjPostPaymentSheet, int BranchId, int Uid, string Module)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLocationIds", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.LocationName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ModuleNameMovement", MySqlDbType = MySqlDbType.VarChar, Value = "Import" });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });


            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditImpInternalMovement", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPG_Internal_Movement ObjInternalMovement = new PPG_Internal_Movement();
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
                    ObjInternalMovement.LocationId = Convert.ToInt32(Result["NewLocationIds"].ToString());
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
        public void DelInternalMovement(int MovementId)
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
        public void AddEditImpInternalMovement(PPG_Internal_Movement ObjInternalMovement)
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


        public void GetBOENoForInternalMovement()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBOENoForInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_Internal_Movement> LstInternalMovement = new List<PPG_Internal_Movement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    int flag = 0;
                    if (LstInternalMovement.Count > 0)
                    {
                        for (int i = 0; i < LstInternalMovement.Count; i++)
                        {
                            if (LstInternalMovement[i].BOENo == Result["BOENo"].ToString())
                            {
                                flag = 1;
                            }
                        }

                    }
                    if (flag == 0)
                    {
                        LstInternalMovement.Add(new PPG_Internal_Movement
                        {



                            BOENo = Result["BOENo"].ToString(),
                            DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"])
                        });
                    }
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

        public void GetBOENoDetForMovement(int DestuffingEntryDtlId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBOENoForInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPG_Internal_Movement ObjInternalMovement = new PPG_Internal_Movement();
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

        #region Delivery Application
        public void GetAllDeliveryApplication(int Page, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDelivaryAppForPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPGDeliveryApplicationList> LstDeliveryApp = new List<PPGDeliveryApplicationList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDeliveryApp.Add(new PPGDeliveryApplicationList
                    {
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DeliveryNo = Result["DeliveryNo"].ToString(),
                        DeliveryAppDate = Result["DeliveryAppDate"].ToString(),
                        DeliveryId = Convert.ToInt32(Result["DeliveryId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDeliveryApp;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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


        public void ListOfIssueParty()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetListOfIssue", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            List<VRN_DeliveryOrdDtl> LstStuffing = new List<VRN_DeliveryOrdDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new VRN_DeliveryOrdDtl
                    {

                        IssuedBy = Result["EximTraderName"].ToString()


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
        public void GetDeliveryApplication(int DeliveryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DeliveryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDeliveryApplication", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VRN_DeliverApplication ObjDeliveryApp = new VRN_DeliverApplication();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDeliveryApp.DeliveryId = Convert.ToInt32(Result["DeliveryId"]);
                    ObjDeliveryApp.DeliveryNo = Result["DeliveryNo"].ToString();
                    ObjDeliveryApp.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    ObjDeliveryApp.DestuffingId = Convert.ToInt32(Result["DestuffingId"]);
                    //ObjDeliveryApp.ImporterId = Convert.ToInt32(Result["ImporterId"]);
                    ObjDeliveryApp.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjDeliveryApp.CHA = Result["CHA"].ToString();
                    //ObjDeliveryApp.Importer = Result["Importer"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryApp.LstDeliveryAppDtl.Add(new VRN_DeliveryApplicationDtl
                        {
                            DeliveryDtlId = Convert.ToInt32(Result["DeliveryDtlId"]),
                            DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["OBL"] == null ? "" : Result["OBL"]).ToString(),
                            OOC_BOENo = (Result["BOE_NO"] == null ? "" : Result["BOE_NO"]).ToString(),
                            OOC_BOEDATE = (Result["BOE_DATE"] == null ? "" : Result["BOE_DATE"]).ToString(),
                            BOELineNo = (Result["BOELineNo"] == null ? "" : Result["BOELineNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            Importer = Result["Importer"].ToString(),
                            ImporterId = Convert.ToInt32(Result["ImporterId"]),
                            //  Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            // CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]),
                            CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]),
                            SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"] == DBNull.Value ? 0 : Result["GrossWt"]),
                            CIF = Convert.ToDecimal(Result["CIF"] == DBNull.Value ? 0 : Result["CIF"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            DelCUM = Convert.ToDecimal(Result["DelCUM"] == DBNull.Value ? 0 : Result["DelCUM"]),
                            DelSQM = Convert.ToDecimal(Result["DelSQM"] == DBNull.Value ? 0 : Result["DelSQM"]),
                            DelGrossWt = Convert.ToDecimal(Result["DelGrossWt"] == DBNull.Value ? 0 : Result["DelGrossWt"]),
                            DelCIF = Convert.ToDecimal(Result["DelCIF"] == DBNull.Value ? 0 : Result["DelCIF"]),
                            DelDuty = Convert.ToDecimal(Result["DelDuty"] == DBNull.Value ? 0 : Result["DelDuty"]),
                            DelNoOfPackages = Convert.ToInt32(Result["DelNoOfPackages"] == DBNull.Value ? 0 : Result["DelNoOfPackages"])
                        });
                    }
                }



                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryApp.LstDeliveryordDtl.Add(new VRN_DeliveryOrdDtl
                        {
                            OrderId = Convert.ToInt32(Result["OrderId"]),
                            DeliveryNo = Result["DeliveryNo"].ToString(),
                            IssuedBy = (Result["DoIssueBy"] == null ? "" : Result["DoIssueBy"]).ToString(),
                            DeliveredTo = (Result["CargsDelivered"] == null ? "" : Result["CargsDelivered"]).ToString(),
                            ValidType = (Result["ValidType"] == null ? "" : Result["ValidType"]).ToString(),
                            ValidDate = (Result["ValidDate"] == null ? "" : Result["ValidDate"]).ToString()
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDeliveryApp;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void AddEditDeliveryApplication(PPG_DeliverApplication ObjDeliveryApp, string DeliveryXml, String ObjDeliveryOrd)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliveryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryXML", MySqlDbType = MySqlDbType.Text, Value = DeliveryXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrdXML", MySqlDbType = MySqlDbType.Text, Value = ObjDeliveryOrd });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.CHAId });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.ImporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditDeliveryApplication", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Delivery Application Details Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Delivery Application Details Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Delivery Application Details As It Already Exist In Another Page";
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


        public void GetCIFFromOOCDelivery(String BOE, String BOEDT)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = BOE });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(BOEDT).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCIFDutyForBOE", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CIFFromOOC CIFFromOOCOBJ = new CIFFromOOC();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    CIFFromOOCOBJ.CIF = Result["CIF_VALUE"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIF_VALUE"]);
                    CIFFromOOCOBJ.Duty = Result["DUTY"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["DUTY"]);
                    CIFFromOOCOBJ.BOE_DATE = Convert.ToString(Result["BOE_DATE"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CIFFromOOCOBJ;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetBOELineNoForDelivery(int DestuffingId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpBOELineNoForDelivery", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VRN_BOELineNoList> LstBOELineNo = new List<VRN_BOELineNoList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBOELineNo.Add(new VRN_BOELineNoList
                    {
                        BOELineNo = Result["BOELineNo"].ToString(),
                        DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                        CHA = Convert.ToString(Result["CHA"]),
                        CHAId = Convert.ToString(Result["CHAId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBOELineNo;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetBOELineNoDetForDelivery(int DestuffingEntryDtlId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpBOELineNoDetForDelivery", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VRN_DeliveryApplicationDtl ObjDeliveryApp = new VRN_DeliveryApplicationDtl();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDeliveryApp.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjDeliveryApp.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    // ObjDeliveryApp.CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]);
                    ObjDeliveryApp.SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]);
                    ObjDeliveryApp.GrossWt = Convert.ToDecimal(Result["GrossWt"] == DBNull.Value ? 0 : Result["GrossWt"]);
                    ObjDeliveryApp.Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]);
                    ObjDeliveryApp.CIF = Convert.ToDecimal(Result["CIF"] == DBNull.Value ? 0 : Result["CIF"]);
                    ObjDeliveryApp.OOC_BOENo = Result["BOENo"].ToString();
                    ObjDeliveryApp.OOC_BOEDATE = Result["BOEDate"] == DBNull.Value ? "" : Convert.ToDateTime(Result["BOEDate"]).ToString("dd/MM/yyyy");
                    ObjDeliveryApp.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjDeliveryApp.Importer = Convert.ToString(Result["Importer"] == DBNull.Value ? "" : Result["Importer"]);
                    ObjDeliveryApp.ShippingLine = Convert.ToString(Result["ShippingLineName"] == DBNull.Value ? "" : Result["ShippingLineName"]);
                    // ObjDeliveryApp.Commodity = Result["Commodity"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryApp.OblFreeFlag = Convert.ToInt32(Result["Flag"].ToString());
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDeliveryApp;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetDestuffEntryNo(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Uid) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffEntryNo", CommandType.StoredProcedure, DParam);
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
                        DestuffingId = Convert.ToInt32(Result["DestuffingId"]),
                        ImporterName = Result["ImporterName"].ToString(),
                        ImporterId = Convert.ToInt32(Result["ImporterId"])
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

        #region Payment Sheet Godown
        public void GetDeliveryApplicationForImpPaymentSheet(int DestuffingAppId = 0)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDeliveryApplicationForImpPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaySheetStuffingRequest> objPaySheetStuffing = new List<PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new PaySheetStuffingRequest()
                    {
                        CHAId = Result["CHAId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CHAId"]),
                        CHAName = Result["CHAName"] == System.DBNull.Value ? "" : Convert.ToString(Result["CHAName"]),
                        CHAGSTNo = Result["GSTNo"] == System.DBNull.Value ? "" : Convert.ToString(Result["GSTNo"]),
                        StuffingReqId = Result["DeliveryId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["DeliveryId"]),
                        StuffingReqNo = Result["DeliveryNo"] == System.DBNull.Value ? "" : Convert.ToString(Result["DeliveryNo"]),
                        StuffingReqDate = Result["DeliveryAppDate"] == System.DBNull.Value ? "" : Convert.ToString(Result["DeliveryAppDate"]),
                        Address = Result["Address"] == System.DBNull.Value ? "" : Convert.ToString(Result["Address"]),
                        DeliveryType = Result["DeliveryType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["DeliveryType"]),
                        State = Result["StateName"] == System.DBNull.Value ? "" : Convert.ToString(Result["StateName"]),
                        StateCode = Result["StateCode"] == System.DBNull.Value ? "" : Convert.ToString(Result["StateCode"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetBOEForDeliveryPaymentSheet(int DestuffingAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDeliveryApplicationForImpPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<VRN_PaymentSheetBOE> objPaymentSheetBOE = new List<VRN_PaymentSheetBOE>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetBOE.Add(new VRN_PaymentSheetBOE()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        LineNo = Convert.ToString(Result["LineNo"]),
                        BOENo = Convert.ToString(Result["BOENo"]),
                        //  BOEDate = Convert.ToString(Result["BOEDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheetBOE;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetBOEForPaymentSheet(string ContainerXML)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBOENoForImpDeliveryPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaymentSheetBOE> objPaymentSheetBOE = new List<PaymentSheetBOE>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetBOE.Add(new PaymentSheetBOE()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        LineNo = Convert.ToString(Result["LineNo"]),
                        BOENo = Convert.ToString(Result["BOENo"]),
                        BOEDate = Convert.ToString(Result["BOEDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheetBOE;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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


        public void GetPaymentParty()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyForImport", CommandType.StoredProcedure);
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




        public void GetImpPaymentPartyForMergePage(string PartyCode)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            //   lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPartyForMergePage", CommandType.StoredProcedure, Dparam);
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

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstParty;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetDeliveryPaymentSheet_Patparganj(string InvoiceDate, string ExportUnder, int DestuffingAppId, int DeliveryType, string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,
            string InvoiceType, string LineXML, int InvoiceId = 0, int OTHours = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "LineXML", MySqlDbType = MySqlDbType.Text, Value = LineXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Value = OTHours });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();


            VRN_InvoiceGodown objInvoice = new VRN_InvoiceGodown();
            IDataReader Result = DataAccess.ExecuteDataReader("getdeliverypaymentsheet", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["DestuffingId"]);
                        objInvoice.RequestNo = Result["DestuffingNo"].ToString();
                        objInvoice.RequestDate = Result["DestuffingDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();

                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPrePaymentCont.Add(new VRN_PreInvoiceContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWeight = Result["GrossWeight"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerPack = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Result["CIFValue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Result["DelDuty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["DelDuty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Result["RMS"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Result["DeliveryType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Result["Insured"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Result["HeavyScrap"] == DBNull.Value ? 0 : Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Result["AppraisementPerct"] == DBNull.Value ? 0 : Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = " ",
                            CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ApproveOn = "0",
                            BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            OperationType = 0,
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new VRN_PostPaymentContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["DelDuty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Result["RMS"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Result["DeliveryType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Result["Insured"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Result["HeavyScrap"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = " ",
                            CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContWiseAmount.Add(new VRN_ContainerWiseAmount
                        {
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            EntryFee = Result["EntryFee"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Result["CstmRevenue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Result["GrEmpty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Result["GrLoaded"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Result["ReeferCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Result["StorageCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Result["InsuranceCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Result["PortCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Result["WeighmentCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WeighmentCharge"]),
                            ContainerId = 0,
                            InvoiceId = 0,
                            LineNo = ""
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstInvoiceCargo.Add(new VRN_InvoiceCargo
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
                            GodownWiseLctnNames = Result["GodownWiseLctnNames"].ToString(),
                            GodownWiseLocationIds = Result["GodownWiseLocationIds"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new VRN_PostPaymentChrg
                        {
                            ChargeId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"] == System.DBNull.Value ? "" : Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                            Discount = Result["Discount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Discount"]),
                            Taxable = Result["Taxable"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Result["IGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Result["IGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Result["SGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Result["SGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Result["CGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Result["CGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Result["Total"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Total"]),
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                        });
                    }


                }

                if (Result.NextResult())
                {
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new VRN_OperationCFSCodeWiseAmount
                        {
                            InvoiceId = InvoiceId,
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"] == System.DBNull.Value ? "" : Result["ContainerNo"].ToString(),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    if (Result.Read())
                    {
                        objInvoice.hasSDAvailableBalance = Convert.ToInt32(Result["hasSDAvailableBalance"]);
                    }
                }
                if (Result.NextResult())
                {
                    if (Result.Read())
                    {
                        objInvoice.PaymentMode = Result["IN_MODE"].ToString();
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        objInvoice.lstPostPaymentChrgBreakup.Add(new VRN_DeliPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            Startdate = Result["StartDate"].ToString(),
                            EndDate = Result["EndDate"].ToString(),

                        });
                    }


                }
                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

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



        public void AddEditInvoiceGodown(VRN_InvoiceGodown ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
       string ChargesBreakupXML, int BranchId, int Uid,
          string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
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
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditExpInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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
                else if (Result == 5)
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


        public void AddEditInvoiceGodownMerge(VRN_InvoiceGodown ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
       string ChargesBreakupXML, int BranchId, int Uid,
       string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjPostPaymentSheet.ArrivalDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });

            // LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            ppgmergedet pdet = new ppgmergedet();
            int Result = DA.ExecuteNonQuery("AddEditExpInvoiceMerge", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);

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
                else if (Result == 5)
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

        #region form one
        public void AddEditFormOne(PPG_FormOneModel objFormOne, int BranchId, string FormOneDetailXML, int CreatedBy)
        {
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.FormOneId });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_BLNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.BLNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VesselName", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.VesselName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VoyageNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.VoyageNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RotationNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.RotationNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortOfDischargeId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.PortOfDischargeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortCharge", MySqlDbType = MySqlDbType.Decimal, Value = objFormOne.PortCharge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortChargeAmt", MySqlDbType = MySqlDbType.Decimal, Value = objFormOne.PortChargeAmt });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.CargoType });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.LCLFCL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneDetailXML", MySqlDbType = MySqlDbType.Text, Value = FormOneDetailXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = CreatedBy });

            LstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "@GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("AddEditFormOne", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Form-1 Saved Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Form-1 Updated Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Form-1 Details As It Already Exists In Another Page";
                    _DBResponse.Data = null;
                }
                else if (Result == -1)
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
        public void DeleteFormOne(int FormOneId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int Result = DataAccess.ExecuteNonQuery("DeleteFormOne", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Form-1 Data Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Form-1 Data Does Not Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Form-1 Details As It Already Exists In Another Page";
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
        public void ListOfShippingLineForm()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "ShippingLine" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
            IList<ShippingLine> lstShippingLine = new List<ShippingLine>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new ShippingLine
                    {
                        ShippingLineId = Convert.ToInt32(result["EximTraderId"]),
                        ShippingLineName = result["NameAddress"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstShippingLine;
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
        public void ListOfCHAForm()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "CHA" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
            IList<CHA> lstCHA = new List<CHA>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHA
                    {
                        CHAId = Convert.ToInt32(result["EximTraderId"]),
                        CHAName = result["NameAddress"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstCHA;
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
        public void ListOfImporterForm(int Page, string PartyCode)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "Importer" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Value = PartyCode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
            IList<VRN_Importer> lstImporter = new List<VRN_Importer>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (result.Read())
                {
                    Status = 1;
                    lstImporter.Add(new VRN_Importer
                    {
                        ImporterId = Convert.ToInt32(result["EximTraderId"]),
                        ImporterName = result["NameAddress"].ToString(),
                        ImporterCode = Convert.ToString(result["EximTraderAlias"])

                    });
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        State = Convert.ToBoolean(result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = new { lstImporter, State };
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

        public void ListOfPODForm()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfPOD", CommandType.StoredProcedure);
            IList<PortOfDischarge> lstPOD = new List<PortOfDischarge>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstPOD.Add(new PortOfDischarge
                    {
                        PODId = Convert.ToInt32(result["PortId"]),
                        PODName = result["PortName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstPOD;
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
        public void ListOfCommodityForm()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetMstCommodity", CommandType.StoredProcedure, DParam);
            IList<PPG_Commodity> lstCommodity = new List<PPG_Commodity>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCommodity.Add(new PPG_Commodity
                    {
                        CommodityId = Convert.ToInt32(result["CommodityId"]),
                        CommodityName = result["CommodityName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstCommodity;
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
        public void GetFormOne()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormOne", CommandType.StoredProcedure, DParam);
            IList<PPG_FormOneModel> lstFormOne = new List<PPG_FormOneModel>();
            int Status = 0;

            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new PPG_FormOneModel
                    {
                        FormOneId = Convert.ToInt32(result["FormOneId"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        ShippingLineName = Convert.ToString(result["EximTraderName"]),
                        TrBondNo = Convert.ToString(result["TrBondNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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


        public void GetFormOneByContainer(string ContainerName)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_containerNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ContainerName });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormBYContainer", CommandType.StoredProcedure, DParam);
            IList<PPG_FormOneModel> lstFormOne = new List<PPG_FormOneModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new PPG_FormOneModel
                    {
                        FormOneId = Convert.ToInt32(result["FormOneId"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        ShippingLineName = Convert.ToString(result["EximTraderName"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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
        public void GetFormOneById(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormOne", CommandType.StoredProcedure, DParam);
            PPG_FormOneModel objFormOne = new PPG_FormOneModel();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objFormOne.FormOneId = Convert.ToInt32(result["FormOneId"]);
                    objFormOne.FormOneNo = Convert.ToString(result["FormOneNo"]);
                    objFormOne.FormOneDate = Convert.ToString(result["FormOneDate"]);
                    objFormOne.ShippingLineId = Convert.ToInt32(result["ShippingLineId"]);
                    objFormOne.ShippingLineName = Convert.ToString(result["EximTraderName"]);
                    objFormOne.VesselName = Convert.ToString(result["VesselName"]);
                    objFormOne.VoyageNo = Convert.ToString(result["VoyageNo"]);
                    objFormOne.RotationNo = Convert.ToString(result["RotationNo"]);
                    objFormOne.PortOfDischargeId = Convert.ToInt32(result["PortOfDischargeId"]);
                    objFormOne.PortName = Convert.ToString(result["PortName"]);
                    objFormOne.PortCharge = Convert.ToDecimal(result["PortCharge"]);
                    objFormOne.PortChargeAmt = Convert.ToDecimal(result["PortChargeAmount"]);
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objFormOne.lstFormOneDetail.Add(new PPG_FormOneDetail()
                        {
                            FormOneDetailID = Convert.ToInt32(result["FormOneDetailId"]),
                            ContainerNo = Convert.ToString(result["ContainerNo"]),
                            ContainerSize = Convert.ToString(result["ContainerSize"]),
                            Reefer = Convert.ToInt32(result["Reefer"]),
                            FlatReck = Convert.ToInt32(result["FlatReck"]),
                            SealNo = Convert.ToString(result["SealNo"]),
                            LineNo = Convert.ToString(result["LineNo"]),
                            MarksNo = Convert.ToString(result["MarksNo"]),
                            CHAId = Convert.ToInt32(result["CHAId"]),
                            CHAName = Convert.ToString(result["CHAName"]),
                            ImporterId = Convert.ToInt32(result["ImporterId"]),
                            ImporterName = Convert.ToString(result["ImporterName"]),
                            CargoDesc = Convert.ToString(result["CargoDesc"]),
                            CommodityId = Convert.ToInt32(result["CommodityId"]),
                            CommodityName = Convert.ToString(result["CommodityName"]),
                            CargoType = Convert.ToInt32(result["CargoType"]),
                            DateOfLanding = Convert.ToString(result["DateOfLanding"]),
                            Remarks = Convert.ToString(result["Remarks"]),
                            BLNo = Convert.ToString(result["BLNo"]),
                            LCLFCL = Convert.ToString(result["LCLFCL"]),


                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objFormOne;
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
        public void FormOnePrint(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("FormOnePrint", CommandType.StoredProcedure, DParam);
            Kol_FormOnePrintModel objFormOne = new Kol_FormOnePrintModel();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objFormOne.ShippingLineNo = Convert.ToString(result["ShippingLine"]);
                    objFormOne.FormOneNo = Convert.ToString(result["CustomSlNo"]);
                    objFormOne.FormOneDate = Convert.ToString(result["FormOneDate"]);
                    objFormOne.CHAName = Convert.ToString(result["EximTraderName"]);
                    objFormOne.CHAAddress = Convert.ToString(result["Address"]);
                    objFormOne.CHAPhoneNo = Convert.ToString(result["PhoneNo"]);
                    objFormOne.ShippingLineNo = result["ShippingLine"].ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objFormOne.lstFormOnePrintDetail.Add(new Kol_FormOnePrintDetailModel()
                        {
                            VesselName = Convert.ToString(result["VesselName"]),
                            VoyageNo = Convert.ToString(result["VoyageNo"]),
                            RotationNo = Convert.ToString(result["RotationNo"]),
                            ContainerNo = Convert.ToString(result["ContainerNo"]),
                            SealNo = Convert.ToString(result["SealNo"]),
                            ImpName = Convert.ToString(result["EximTraderName"]),
                            ImpAddress = Convert.ToString(result["Address"]),
                            ImpName2 = Convert.ToString(result["ImporterParty2"]),
                            ImpAddress2 = Convert.ToString(result["ImporterPartyAddress2"]),
                            Type = Convert.ToString(result["TypeLoadEmpty"]),
                            LineNo = Convert.ToString(result["LineNo"]),
                            CargoDesc = Convert.ToString(result["CargoDesc"]),
                            DateOfLanding = Convert.ToString(result["DateOfLanding"]),
                            HazType = Convert.ToInt32(result["HazType"]) == 2 ? "NON-HAZ" : "HAZ",
                            ReferType = Convert.ToInt32(result["ReferType"]) == 0 ? "" : "/ REEFER"
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objFormOne;
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

        #region FCL To LCL Conversion 
        public void ListOfContainerFCLtoLCL()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerFCLtoLCLConversion", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.FCLtoLCLContainerList> lstContainerList = new List<Areas.Import.Models.FCLtoLCLContainerList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstContainerList.Add(new Areas.Import.Models.FCLtoLCLContainerList
                    {
                        ContainerId = Result["ContainerId"] == DBNull.Value ? 0 : Convert.ToInt32(Result["ContainerId"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Size = Convert.ToString(Result["Size"]),
                        GateInDate = Convert.ToString(Result["EntryDateTime"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerClassId = Result["CargoType"] == DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                        SALId = Result["ShippingLineId"] == DBNull.Value ? 0 : Convert.ToInt32(Result["ShippingLineId"]),
                        SAL = Convert.ToString(Result["ShippingLine"]),
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
        public void GetPartyPdaForFCLtoLCL()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("GetPartyPdaForFCLtoLCL", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                IList<Areas.Import.Models.FCLtoLCLForwarderList> lstForwarderList = new List<Areas.Import.Models.FCLtoLCLForwarderList>();
                while (result.Read())
                {
                    Status = 1;
                    lstForwarderList.Add(new Areas.Import.Models.FCLtoLCLForwarderList
                    {
                        PartyPdaId = Convert.ToInt32(result["EximTraderId"]),
                        PartyPdaCode = Convert.ToString(result["EximTraderName"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Successful";
                    _DBResponse.Data = lstForwarderList;
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
        public void GetSLA()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("GetSLAForFCLtoLCL", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                IList<Areas.Import.Models.FCLtoLCLSLAList> lstFCLtoLCLSLAList = new List<Areas.Import.Models.FCLtoLCLSLAList>();
                while (result.Read())
                {
                    Status = 1;
                    lstFCLtoLCLSLAList.Add(new Areas.Import.Models.FCLtoLCLSLAList
                    {
                        SLAId = Convert.ToInt32(result["EximTraderId"]),
                        SLA = Convert.ToString(result["EximTraderName"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Successful";
                    _DBResponse.Data = lstFCLtoLCLSLAList;
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
        public void GetPartyPdaDetailsForFCLtoLCL(string Size, int PartyPdaId, int ContainerClassId, String CFSCode)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.VarChar, Value = PartyPdaId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerClassId", MySqlDbType = MySqlDbType.VarChar, Value = ContainerClassId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("GetPartyPdaDetailsForFCLtoLCL", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                FCLtoLCLConversion ObjSC = new FCLtoLCLConversion();
                while (result.Read())
                {

                    Status = 1;
                    ObjSC.DSTFOperationId = result["DTFOperationId"] == DBNull.Value ? 0 : Convert.ToInt32(result["DTFOperationId"]);
                    ObjSC.DSTFChargeType = Convert.ToString(result["DTFChargeType"]);
                    ObjSC.DSTFChargeName = Convert.ToString(result["DTFChargeName"]);
                    ObjSC.DSTFCharge = result["DTFAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(result["DTFAmount"]);
                    ObjSC.DSTFCGSTCharge = result["DTFCGSTAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(result["DTFCGSTAmt"]);
                    ObjSC.DSTFSGSTCharge = result["DTFSGSTAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(result["DTFSGSTAmt"]);
                    ObjSC.DSTFIGSTCharge = result["DTFIGSTAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(result["DTFIGSTAmt"]);
                    ObjSC.DSTFIGSTPer = result["DTFIGSTPer"] == DBNull.Value ? 0 : Convert.ToDecimal(result["DTFIGSTPer"].ToString());
                    ObjSC.DSTFCGSTPer = result["DTFCGSTPer"] == DBNull.Value ? 0 : Convert.ToDecimal(result["DTFCGSTPer"].ToString());
                    ObjSC.DSTFSGSTPer = result["DTFSGSTPer"] == DBNull.Value ? 0 : Convert.ToDecimal(result["DTFSGSTPer"]);
                    ObjSC.DSTFAmount = result["DTFAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(result["DTFAmount"]);
                    ObjSC.DSTFTaxable = result["DTFTaxable"] == DBNull.Value ? 0 : Convert.ToDecimal(result["DTFTaxable"]);
                    ObjSC.DSTFTotalAmount = result["DTFTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(result["DTFTotal"]);
                    ObjSC.DSTFSACCode = Convert.ToString(result["DTFSACCode"]);
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        Status = 1;

                        ObjSC.AmmendOperationId = result["AMDOperationId"] == DBNull.Value ? 0 : Convert.ToInt32(result["AMDOperationId"]);
                        ObjSC.AmmendChargeType = Convert.ToString(result["AMDChargeType"]);
                        ObjSC.AmmendChargeName = Convert.ToString(result["AMDChargeName"]);
                        ObjSC.AmmendCharge = result["AMDAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(result["AMDAmount"]);
                        ObjSC.AmmendCGSTCharge = result["AMDCGSTAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(result["AMDCGSTAmt"]);
                        ObjSC.AmmendSGSTCharge = result["AMDSGSTAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(result["AMDSGSTAmt"]);
                        ObjSC.AmmendIGSTCharge = result["AMDIGSTAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(result["AMDIGSTAmt"]);
                        ObjSC.AmmendIGSTPer = result["AMDIGSTPer"] == DBNull.Value ? 0 : Convert.ToDecimal(result["AMDIGSTPer"].ToString());
                        ObjSC.AmmendCGSTPer = result["AMDCGSTPer"] == DBNull.Value ? 0 : Convert.ToDecimal(result["AMDCGSTPer"].ToString());
                        ObjSC.AmmendSGSTPer = result["AMDSGSTPer"] == DBNull.Value ? 0 : Convert.ToDecimal(result["AMDSGSTPer"]);
                        ObjSC.AmmendAmount = result["AMDAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(result["AMDAmount"]);
                        ObjSC.AmmendTaxable = result["AMDTaxable"] == DBNull.Value ? 0 : Convert.ToDecimal(result["AMDTaxable"]);
                        ObjSC.AmmendTotalAmount = result["AMDTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(result["AMDTotal"]);
                        ObjSC.AmmendSACCode = Convert.ToString(result["AMDSACCode"]);
                    }
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {

                        ObjSC.lstPostPaymentChrgBreakup.Add(new ppgFCLPostPaymentChargebreakupdate
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
        public void AddFCLtoLCLConversion(FCLtoLCLConversion Obj, string ChargesBreakupXML, int BranchId, int Uid)
        {
            try
            {
                String ReturnObj = "";
                String GeneratedClientId = "";
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "In_FCLtoLCLConversionId", MySqlDbType = MySqlDbType.Int32, Value = Obj.FCLtoLCLConversionId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = Obj.ContainerId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.ContainerNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = Obj.CFSCode });
                LstParam.Add(new MySqlParameter { ParameterName = "In_Size", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Size });
                LstParam.Add(new MySqlParameter { ParameterName = "In_GateInDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Obj.GateInDate) });
                LstParam.Add(new MySqlParameter { ParameterName = "In_SALId", MySqlDbType = MySqlDbType.Int32, Value = Obj.SALId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_SAL", MySqlDbType = MySqlDbType.VarChar, Value = Obj.SAL });
                LstParam.Add(new MySqlParameter { ParameterName = "In_ContainerClassId", MySqlDbType = MySqlDbType.Int32, Value = Obj.ContainerClassId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_OldOBLType", MySqlDbType = MySqlDbType.VarChar, Value = Obj.OldOBLType });
                LstParam.Add(new MySqlParameter { ParameterName = "In_NewOBLType", MySqlDbType = MySqlDbType.VarChar, Value = Obj.NewOBLType });
                LstParam.Add(new MySqlParameter { ParameterName = "In_PartyPdaId", MySqlDbType = MySqlDbType.Int32, Value = Obj.PartyPdaId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_PartyPdaCode", MySqlDbType = MySqlDbType.VarChar, Value = Obj.PartyPdaCode });

                LstParam.Add(new MySqlParameter { ParameterName = "In_DSTFOperationId", MySqlDbType = MySqlDbType.Int32, Value = Obj.DSTFOperationId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DSTFChargeType", MySqlDbType = MySqlDbType.VarChar, Value = Obj.DSTFChargeType });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DSTFChargeName", MySqlDbType = MySqlDbType.VarChar, Value = Obj.DSTFChargeName });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DSTFCharge", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DSTFCharge });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DSTFCGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DSTFCGSTCharge });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DSTFSGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DSTFSGSTCharge });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DSTFIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DSTFIGSTCharge });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DSTFIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DSTFIGSTPer });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DSTFCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DSTFCGSTPer });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DSTFSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DSTFSGSTPer });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DSTFTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = Obj.DSTFTotalAmount });
                LstParam.Add(new MySqlParameter { ParameterName = "In_DSTFSACCode", MySqlDbType = MySqlDbType.VarChar, Value = Obj.DSTFSACCode });

                LstParam.Add(new MySqlParameter { ParameterName = "In_AmmendOperationId", MySqlDbType = MySqlDbType.Int32, Value = Obj.AmmendOperationId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_AmmendChargeType", MySqlDbType = MySqlDbType.VarChar, Value = Obj.AmmendChargeType });
                LstParam.Add(new MySqlParameter { ParameterName = "In_AmmendChargeName", MySqlDbType = MySqlDbType.VarChar, Value = Obj.AmmendChargeName });
                LstParam.Add(new MySqlParameter { ParameterName = "In_AmmendCharge", MySqlDbType = MySqlDbType.Decimal, Value = Obj.AmmendCharge });
                LstParam.Add(new MySqlParameter { ParameterName = "In_AmmendCGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = Obj.AmmendCGSTCharge });
                LstParam.Add(new MySqlParameter { ParameterName = "In_AmmendSGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = Obj.AmmendSGSTCharge });
                LstParam.Add(new MySqlParameter { ParameterName = "In_AmmendIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = Obj.AmmendIGSTCharge });
                LstParam.Add(new MySqlParameter { ParameterName = "In_AmmendIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Obj.AmmendIGSTPer });
                LstParam.Add(new MySqlParameter { ParameterName = "In_AmmendCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Obj.AmmendCGSTPer });
                LstParam.Add(new MySqlParameter { ParameterName = "In_AmmendSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Obj.AmmendSGSTPer });
                LstParam.Add(new MySqlParameter { ParameterName = "In_AmmendTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = Obj.AmmendTotalAmount });
                LstParam.Add(new MySqlParameter { ParameterName = "In_AmmendSACCode", MySqlDbType = MySqlDbType.VarChar, Value = Obj.AmmendSACCode });

                LstParam.Add(new MySqlParameter { ParameterName = "In_TotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = Obj.TotalAmount });
                LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });

                LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = Obj.InvoiceId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
                LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("AddEditFCLtoLCLConversion", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
                _DBResponse = new DatabaseResponse();
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "";
                    _DBResponse.Data = Result;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "";
                    _DBResponse.Data = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "";
                    _DBResponse.Data = ReturnObj;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "";
                    _DBResponse.Data = ReturnObj;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "";
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
        public void GetListOfFCLToLCLConversionDtl()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("GetListOfFCLToLCLConversionDtl", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                IList<Areas.Import.Models.FCLtoLCLConversion> lstForwarderList = new List<Areas.Import.Models.FCLtoLCLConversion>();
                while (result.Read())
                {
                    Status = 1;
                    lstForwarderList.Add(new Areas.Import.Models.FCLtoLCLConversion
                    {
                        FCLtoLCLConversionId = Convert.ToInt32(result["FCLtoLCLConversionId"]),
                        ContainerId = Convert.ToInt32(result["ContainerId"]),
                        ContainerNo = Convert.ToString(result["ContainerNo"]),
                        Size = Convert.ToString(result["Size"]),
                        GateInDate = Convert.ToString(result["GateInDate"]),
                        CFSCode = Convert.ToString(result["CFSCode"]),
                        ContainerClassId = Convert.ToInt32(result["ContainerClassId"]),
                        ContainerClass = Convert.ToString(result["ContainerClass"]),
                        SALId = Convert.ToInt32(result["SALId"]),
                        SAL = Convert.ToString(result["SAL"]),
                        OldOBLType = Convert.ToString(result["OldOBLType"]),
                        NewOBLType = Convert.ToString(result["NewOBLType"]),
                        PartyPdaId = Convert.ToInt32(result["PartyPdaId"]),
                        PartyPdaCode = Convert.ToString(result["PartyPdaCode"]),
                        DSTFCharge = Convert.ToDecimal(result["DSTFCharge"]),
                        DSTFCGSTCharge = Convert.ToDecimal(result["DSTFCGSTCharge"]),
                        DSTFSGSTCharge = Convert.ToDecimal(result["DSTFSGSTCharge"]),
                        DSTFIGSTCharge = Convert.ToDecimal(result["DSTFIGSTCharge"]),
                        DSTFTotalAmount = Convert.ToDecimal(result["DSTFTotalAmount"]),
                        DSTFIGSTPer = Convert.ToDecimal(result["DSTFIGSTPer"]),
                        DSTFCGSTPer = Convert.ToDecimal(result["DSTFCGSTPer"]),
                        DSTFSGSTPer = Convert.ToDecimal(result["DSTFSGSTPer"]),
                        AmmendCharge = Convert.ToDecimal(result["AmmendCharge"]),
                        AmmendCGSTCharge = Convert.ToDecimal(result["AmmendCGSTCharge"]),
                        AmmendSGSTCharge = Convert.ToDecimal(result["AmmendSGSTCharge"]),
                        AmmendIGSTCharge = Convert.ToDecimal(result["AmmendIGSTCharge"]),
                        AmmendTotalAmount = Convert.ToDecimal(result["AmmendTotalAmount"]),
                        AmmendIGSTPer = Convert.ToDecimal(result["AmmendIGSTPer"]),
                        AmmendCGSTPer = Convert.ToDecimal(result["AmmendCGSTPer"]),
                        AmmendSGSTPer = Convert.ToDecimal(result["AmmendSGSTPer"]),
                        InvoiceId = Convert.ToInt32(result["InvoiceId"]),
                        InvoiceNo = Convert.ToString(result["InvoiceNo"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Successful";
                    _DBResponse.Data = lstForwarderList;
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
        public void ViewFCLtoLCLConversionbyId(int FCLtoLCLConversionId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_FCLtoLCLConversionId", MySqlDbType = MySqlDbType.Int32, Value = FCLtoLCLConversionId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("ViewFCLtoLCLConversionbyId", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                FCLtoLCLConversion Obj = new FCLtoLCLConversion();
                while (result.Read())
                {
                    Status = 1;
                    Obj.FCLtoLCLConversionId = Convert.ToInt32(result["FCLtoLCLConversionId"]);
                    Obj.ContainerId = Convert.ToInt32(result["ContainerId"]);
                    Obj.ContainerNo = Convert.ToString(result["ContainerNo"]);
                    Obj.Size = Convert.ToString(result["Size"]);
                    Obj.GateInDate = Convert.ToString(result["GateInDate"]);
                    Obj.CFSCode = Convert.ToString(result["CFSCode"]);
                    Obj.ContainerClassId = Convert.ToInt32(result["ContainerClassId"]);
                    Obj.ContainerClass = Convert.ToString(result["ContainerClass"]);
                    Obj.SALId = Convert.ToInt32(result["SALId"]);
                    Obj.SAL = Convert.ToString(result["SAL"]);
                    Obj.OldOBLType = Convert.ToString(result["OldOBLType"]);
                    Obj.NewOBLType = Convert.ToString(result["NewOBLType"]);
                    Obj.PartyPdaId = Convert.ToInt32(result["PartyPdaId"]);
                    Obj.PartyPdaCode = Convert.ToString(result["PartyPdaCode"]);
                    Obj.DSTFCharge = Convert.ToDecimal(result["DSTFCharge"]);
                    Obj.DSTFChargeName = Convert.ToString(result["DSTFChargeName"]);
                    Obj.DSTFCGSTCharge = Convert.ToDecimal(result["DSTFCGSTCharge"]);
                    Obj.DSTFSGSTCharge = Convert.ToDecimal(result["DSTFSGSTCharge"]);
                    Obj.DSTFIGSTCharge = Convert.ToDecimal(result["DSTFIGSTCharge"]);
                    Obj.DSTFTotalAmount = Convert.ToDecimal(result["DSTFTotalAmount"]);
                    Obj.DSTFIGSTPer = Convert.ToDecimal(result["DSTFIGSTPer"]);
                    Obj.DSTFCGSTPer = Convert.ToDecimal(result["DSTFCGSTPer"]);
                    Obj.DSTFSGSTPer = Convert.ToDecimal(result["DSTFSGSTPer"]);
                    Obj.AmmendCharge = Convert.ToDecimal(result["AmmendCharge"]);
                    Obj.AmmendChargeName = Convert.ToString(result["AmmendChargeName"]);
                    Obj.AmmendCGSTCharge = Convert.ToDecimal(result["AmmendCGSTCharge"]);
                    Obj.AmmendSGSTCharge = Convert.ToDecimal(result["AmmendSGSTCharge"]);
                    Obj.AmmendIGSTCharge = Convert.ToDecimal(result["AmmendIGSTCharge"]);
                    Obj.AmmendTotalAmount = Convert.ToDecimal(result["AmmendTotalAmount"]);
                    Obj.AmmendIGSTPer = Convert.ToDecimal(result["AmmendIGSTPer"]);
                    Obj.AmmendCGSTPer = Convert.ToDecimal(result["AmmendCGSTPer"]);
                    Obj.AmmendSGSTPer = Convert.ToDecimal(result["AmmendSGSTPer"]);
                    Obj.InvoiceId = Convert.ToInt32(result["InvoiceId"]);
                    Obj.InvoiceNo = Convert.ToString(result["InvoiceNo"]);
                    Obj.TotalAmount = Convert.ToDecimal(result["TotalAmount"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Successful";
                    _DBResponse.Data = Obj;
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
        public void DeleteFCLtoLCLConversion(int fCLtoLCLConversionId, int BranchId)
        {
            int RetValue = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(BranchId) });
            LstParam.Add(new MySqlParameter { ParameterName = "In_fCLtoLCLConversionId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(fCLtoLCLConversionId) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = RetValue });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteFCLtoLCLConversion", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "FCL to LCL Conversion Deleted Successfully";
                    _DBResponse.Data = null;
                }
                if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot delete as container no. used in seal cutting";
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

        #endregion

        #region Issue Slip
        public void AddEditIssueSlip(VRN_IssueSlip ObjIssueSlip)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjIssueSlip.IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjIssueSlip.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjIssueSlip.IssueSlipDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjIssueSlip.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjIssueSlip.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditImpIssueSlip", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjIssueSlip.IssueSlipId == 0 ? "Issue Slip Details Saved Successfully" : "Issue Slip Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Issue Slip  Details Already Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Issue Slip  Details As It Already Exists In Another Page";
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




        public void AddEditMergeIssueSlip(VRN_MergeDeliveryIssueViewModel ObjIssueSlip)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjIssueSlip.IssueSlip.IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjIssueSlip.IssueSlip.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjIssueSlip.IssueSlip.IssueSlipDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjIssueSlip.IssueSlip.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjIssueSlip.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 100, Direction = ParameterDirection.Output, Value = ReturnObj });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            VRN_mergedet pdet = new VRN_mergedet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMergeImpIssueSlip", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    pdet.Id = Convert.ToInt32(GeneratedClientId);
                    pdet.AppNo = ReturnObj;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjIssueSlip.IssueSlip.IssueSlipId == 0 ? "Issue Slip Details Saved Successfully" : "Issue Slip Details Updated Successfully";
                    _DBResponse.Data = pdet;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Issue Slip  Details Already Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Issue Slip  Details As It Already Exists In Another Page";
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



        public void DelIssueSlip(int IssueSlipId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelImpIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Issue Slip Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Issue Slip Details As It Exists In Another Page";
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
        public void GetIssueSlip(int IssueSlipId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VRN_IssueSlip ObjIssueSlip = new VRN_IssueSlip();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjIssueSlip.IssueSlipId = Convert.ToInt32(Result["IssueSlipId"]);
                    ObjIssueSlip.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    ObjIssueSlip.IssueSlipNo = Result["IssueSlipNo"].ToString();
                    ObjIssueSlip.IssueSlipDate = Result["IssueSlipDate"].ToString();
                    ObjIssueSlip.CargoDescription = Result["CargoDescription"].ToString();
                    ObjIssueSlip.InvoiceNo = Result["InvoiceNo"].ToString();
                    ObjIssueSlip.InvoiceDate = Result["InvoiceDate"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstContainer.Add(new VRN_IssueSlipContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            Total = Convert.ToDecimal(Result["Total"] == DBNull.Value ? 0 : Result["Total"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstCargo.Add(new VRN_IssueSlipCargo
                        {
                            OBLNo = (Result["OBLNo"] == null ? "" : Result["OBLNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            GodownNo = (Result["GodownNo"] == null ? "" : Result["GodownNo"]).ToString(),
                            Location = (Result["Location"] == null ? "" : Result["Location"]).ToString(),
                            StackNo = (Result["StackNo"] == null ? "" : Result["StackNo"]).ToString(),
                            Area = Convert.ToString(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            NetWeight = Convert.ToDecimal(Result["NetWeight"] == DBNull.Value ? 0 : Result["NetWeight"])
                        });
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjIssueSlip;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetAllIssueSlip(int Page = 0, string SearchValue = "")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VRN_IssueSlip> LstIssueSlip = new List<VRN_IssueSlip>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstIssueSlip.Add(new VRN_IssueSlip
                    {
                        IssueSlipNo = Result["IssueSlipNo"].ToString(),
                        IssueSlipDate = Result["IssueSlipDate"].ToString(),
                        IssueSlipId = Convert.ToInt32(Result["IssueSlipId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstIssueSlip;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetInvoiceNoForIssueSlip()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceNoForIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VRN_IssueSlip> LstIssueSlip = new List<VRN_IssueSlip>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstIssueSlip.Add(new VRN_IssueSlip
                    {
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstIssueSlip;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetInvoiceDetForIssueSlip(int InvoiceId)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetForIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VRN_IssueSlip ObjIssueSlip = new VRN_IssueSlip();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjIssueSlip.InvoiceDate = Result["InvoiceDate"].ToString();
                    ObjIssueSlip.CargoDescription = Result["CargoDescription"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstContainer.Add(new VRN_IssueSlipContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            Size = Result["Size"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),

                            Total = Convert.ToDecimal(Result["Total"] == DBNull.Value ? 0 : Result["Total"])
                        });
                    }

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstCargo.Add(new VRN_IssueSlipCargo
                        {
                            OBLNo = (Result["OBLNo"] == null ? "" : Result["OBLNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            GodownNo = (Result["GodownNo"] == null ? "" : Result["GodownNo"]).ToString(),
                            Location = (Result["Location"] == null ? "" : Result["Location"]).ToString(),
                            StackNo = (Result["StackNo"] == null ? "" : Result["StackNo"]).ToString(),
                            Area = Convert.ToString(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            NetWeight = Convert.ToDecimal(Result["NetWeight"] == DBNull.Value ? 0 : Result["NetWeight"])
                        });
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjIssueSlip;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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


        public void GetInvoiceDetForMergeIssueSlip(String InvoiceNo)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = InvoiceNo });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetForMergeIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VRN_IssueSlip ObjIssueSlip = new VRN_IssueSlip();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjIssueSlip.InvoiceId = Convert.ToInt32(Result["InvoiceId"].ToString());
                    ObjIssueSlip.InvoiceDate = Result["InvoiceDate"].ToString();
                    ObjIssueSlip.CargoDescription = Result["CargoDescription"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstContainer.Add(new VRN_IssueSlipContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            Size = Result["Size"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),

                            Total = Convert.ToDecimal(Result["Total"] == DBNull.Value ? 0 : Result["Total"])
                        });
                    }

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstCargo.Add(new VRN_IssueSlipCargo
                        {
                            OBLNo = (Result["OBLNo"] == null ? "" : Result["OBLNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            GodownNo = (Result["GodownNo"] == null ? "" : Result["GodownNo"]).ToString(),
                            Location = (Result["Location"] == null ? "" : Result["Location"]).ToString(),
                            StackNo = (Result["StackNo"] == null ? "" : Result["StackNo"]).ToString(),
                            Area = Convert.ToString(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            NetWeight = Convert.ToDecimal(Result["NetWeight"] == DBNull.Value ? 0 : Result["NetWeight"])
                        });
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjIssueSlip;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetIssueSlipForPreview(int IssueSlipId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Value = IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("GetIssueSlipForPreview", CommandType.StoredProcedure, DParam);
            VRN_IssueSlip ObjIssueSlip = new VRN_IssueSlip();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjIssueSlip.TotalCWCDues = Convert.ToDecimal(Result["TotalCWCDues"] == DBNull.Value ? 0 : Result["TotalCWCDues"]);
                    ObjIssueSlip.CRNoDate = Convert.ToString(Result["CRNoDate"] == null ? "" : Result["CRNoDate"]);
                    ObjIssueSlip.CompanyLocation = Convert.ToString(Result["CompanyLocation"] == null ? "" : Result["CompanyLocation"]);
                    ObjIssueSlip.CompanyBranch = Convert.ToString(Result["CompanyBranch"] == null ? "" : Result["CompanyBranch"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstIssueSlipRpt.Add(new VRN_IssueSlipReport
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            MarksNo = Convert.ToString(Result["MarksNo"] == null ? "" : Result["MarksNo"]),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString(),
                            Weight = Convert.ToString(Result["Weight"] == null ? "" : Result["Weight"]),
                            ArrivalDate = (Result["ArrivalDate"] == null ? "" : Result["ArrivalDate"]).ToString(),
                            DestuffingDate = (Result["DestuffingDate"] == null ? "" : Result["DestuffingDate"]).ToString(),
                            Location = Result["Location"].ToString(),
                        });
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Data = ObjIssueSlip;
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
                Result.Dispose();
                Result.Close();
            }
        }

        #endregion

        #region Cargo Seize

        public void ListOfOBLNo()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfOBLNoForCargoSeize", CommandType.StoredProcedure);
            IList<CargoSeize> lstCargoSeize = new List<CargoSeize>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCargoSeize.Add(new CargoSeize
                    {
                        DestuffingEntryDtlId = Convert.ToInt32(result["DestuffingEntryDtlId"]),
                        OBLNo = result["OBLNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstCargoSeize;
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

        public void GetCargoSeizeById(int CargoSeizeId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoSeizeId", MySqlDbType = MySqlDbType.Int32, Value = CargoSeizeId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCargoSeizeById", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CargoSeize objCargoSeize = new CargoSeize();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objCargoSeize.Id = Convert.ToInt32(Result["Id"]);
                    objCargoSeize.DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]);
                    objCargoSeize.OBLNo = Convert.ToString(Result["OBLNo"]);
                    objCargoSeize.OBLDate = Convert.ToString(Result["OBLDate"]);
                    objCargoSeize.BOENo = Convert.ToString(Result["BOENo"]);
                    objCargoSeize.BOEDate = Convert.ToString(Result["BOEDate"]);
                    objCargoSeize.LineNo = Convert.ToString(Result["LineNo"]);
                    objCargoSeize.CargoDescription = Convert.ToString(Result["CargoDescription"]);
                    objCargoSeize.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    objCargoSeize.ContainerSize = Convert.ToString(Result["Size"]);
                    objCargoSeize.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objCargoSeize.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    objCargoSeize.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objCargoSeize.CHAId = Convert.ToInt32(Result["CHAId"]);
                    objCargoSeize.CHAName = Convert.ToString(Result["CHAName"]);
                    objCargoSeize.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objCargoSeize.GodownNo = Convert.ToString(Result["GodownNo"]);
                    objCargoSeize.SeizeHoldStatus = Convert.ToInt32(Result["SeizeHoldStatus"]);
                    objCargoSeize.IsSeize = (objCargoSeize.SeizeHoldStatus == 2);
                    objCargoSeize.IsHold = (objCargoSeize.SeizeHoldStatus == 1);
                    objCargoSeize.Remarks = Convert.ToString(Result["Remarks"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objCargoSeize;
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

        public void GetOBLDetails(int DestuffingEntryDtlId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingEntryDtlId });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLDetailsForCargoSeize", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                CargoSeize objCargoSeize = new CargoSeize();

                if (Result != null && Result.Tables.Count > 0)
                {
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        Status = 1;
                        objCargoSeize.OBLDate = Convert.ToString(Result.Tables[0].Rows[0]["OBLDate"]);
                        objCargoSeize.BOENo = Convert.ToString(Result.Tables[0].Rows[0]["BOENo"]);
                        objCargoSeize.BOEDate = Convert.ToString(Result.Tables[0].Rows[0]["BOEDate"]);
                        objCargoSeize.LineNo = Convert.ToString(Result.Tables[0].Rows[0]["LineNo"]);
                        objCargoSeize.CargoDescription = Convert.ToString(Result.Tables[0].Rows[0]["CargoDescription"]);
                        objCargoSeize.ContainerNo = Convert.ToString(Result.Tables[0].Rows[0]["ContainerNo"]);
                        objCargoSeize.ContainerSize = Convert.ToString(Result.Tables[0].Rows[0]["ContainerSize"]);
                        objCargoSeize.CFSCode = Convert.ToString(Result.Tables[0].Rows[0]["CFSCode"]);
                        objCargoSeize.ShippingLineId = Convert.ToInt32(Result.Tables[0].Rows[0]["ShippingLineId"]);
                        objCargoSeize.ShippingLineName = Convert.ToString(Result.Tables[0].Rows[0]["ShippingLineName"]);
                        objCargoSeize.CHAId = Convert.ToInt32(Result.Tables[0].Rows[0]["CHAId"]);
                        objCargoSeize.CHAName = Convert.ToString(Result.Tables[0].Rows[0]["CHAName"]);
                        objCargoSeize.GodownId = Convert.ToInt32(Result.Tables[0].Rows[0]["GodownId"]);
                        objCargoSeize.GodownNo = Convert.ToString(Result.Tables[0].Rows[0]["GodownNo"]);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objCargoSeize;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void AddEditCargoSeize(CargoSeize objCargoSeize)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = objCargoSeize.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Value = objCargoSeize.DestuffingEntryDtlId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLNo", MySqlDbType = MySqlDbType.String, Value = objCargoSeize.OBLNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCargoSeize.OBLDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.String, Value = objCargoSeize.BOENo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCargoSeize.BOEDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_LineNo", MySqlDbType = MySqlDbType.String, Value = objCargoSeize.LineNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.String, Value = objCargoSeize.CargoDescription });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.String, Value = objCargoSeize.ContainerNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.String, Value = objCargoSeize.ContainerSize });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.String, Value = objCargoSeize.CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objCargoSeize.ShippingLineId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = objCargoSeize.CHAId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objCargoSeize.GodownId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SeizeHoldStatus", MySqlDbType = MySqlDbType.Int32, Value = objCargoSeize.SeizeHoldStatus });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.String, Value = objCargoSeize.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OparationType", MySqlDbType = MySqlDbType.String, Value = "Import" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditCargoSeize", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Message = (Result == 1) ? "Cargo Seize Saved Successfully" : "Cargo Seize Updated Successfully";
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

        public void GetAllCargoSeize()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetListCargoSeize", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<CargoSeize> CargoSeizeList = new List<CargoSeize>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        CargoSeize objCargoSeize = new CargoSeize();
                        objCargoSeize.Id = Convert.ToInt32(dr["Id"]);
                        objCargoSeize.OBLNo = Convert.ToString(dr["OBLNo"]);
                        objCargoSeize.OBLDate = Convert.ToString(dr["OBLDate"]);
                        objCargoSeize.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objCargoSeize.ContainerSize = Convert.ToString(dr["Size"].ToString());
                        objCargoSeize.CFSCode = Convert.ToString(dr["CFSCode"]);
                        objCargoSeize.SeizeHoldStatus = Convert.ToInt32(dr["SeizeHoldStatus"]);

                        CargoSeizeList.Add(objCargoSeize);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CargoSeizeList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void DeleteCargoSeize(int CargoSeizeId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoSeizeId", MySqlDbType = MySqlDbType.Int32, Value = CargoSeizeId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("DeleteCargoSeize", CommandType.StoredProcedure, Dparam);
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Cargo Seize Deleted Successfully.";
                    _DBResponse.Status = 1;
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Cargo Seize Can't be Deleted as It Is Used In Carting Work Order.";
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

        #region IRR
        public void GetContainersForIRR(int TrainSummaryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TrainSummaryId", MySqlDbType = MySqlDbType.Int32, Value = TrainSummaryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainersForIRR", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PpgContainerDetailsIRR> objPaySheetStuffing = new List<PpgContainerDetailsIRR>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new PpgContainerDetailsIRR()
                    {
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        DisplayCfs = Convert.ToString(Result["DisplayCfs"]),
                        ShippingLine = Convert.ToString(Result["ShippingLine"]),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        Address = Convert.ToString(Result["Address"]),
                        StateName = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        Size = Convert.ToString(Result["Size"]),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        WagonNo = Convert.ToString(Result["WagonNo"]),
                        Via = Convert.ToString(Result["Via"]),
                        GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                        TareWt = Convert.ToDecimal(Result["TareWt"]),
                        CargoWt = Convert.ToDecimal(Result["CargoWt"]),
                        PortOfOrigin = Convert.ToString(Result["PortOfOrigin"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetTrainListForIRR()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetTrainListForIRR", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<PpgIrrTrains> objTrainList = new List<PpgIrrTrains>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objTrainList.Add(new PpgIrrTrains()
                    {
                        TrainSummaryID = Convert.ToInt32(Result["TrainSummaryID"]),
                        TrainNo = Convert.ToString(Result["trainno"]),
                        TrainNoDate = Convert.ToString(Result["TrainNoDate"]),
                        TrainDate = Convert.ToString(Result["TrainDate"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objTrainList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetIRRPaymentSheet(string InvoiceDate, string CFSCode, string TaxType, int CargoType, int InvoiceId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = TaxType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            VRN_PostPaymentChrg objInvoice = new VRN_PostPaymentChrg();
            IDataReader Result2 = DataAccess.ExecuteDataReader("GetIRRPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {

                while (Result2.Read())
                {
                    objInvoice.ChargeId = Convert.ToInt32(Result2["OperationId"]);
                    objInvoice.Clause = Result2["ChargeType"].ToString();
                    objInvoice.ChargeType = Result2["ChargeType"].ToString();
                    objInvoice.ChargeName = Result2["ChargeName"].ToString();
                    objInvoice.SACCode = Result2["SACCode"].ToString();
                    objInvoice.Quantity = Convert.ToInt32(Result2["Quantity"]);
                    objInvoice.Rate = Convert.ToDecimal(Result2["Rate"]);
                    objInvoice.Amount = Convert.ToDecimal(Result2["Amount"]);
                    objInvoice.Discount = Convert.ToDecimal(Result2["Discount"]);
                    objInvoice.Taxable = Convert.ToDecimal(Result2["Taxable"]);
                    objInvoice.IGSTPer = Convert.ToDecimal(Result2["IGSTPer"]);
                    objInvoice.IGSTAmt = Convert.ToDecimal(Result2["IGSTAmt"]);
                    objInvoice.SGSTPer = Convert.ToDecimal(Result2["SGSTPer"]);
                    objInvoice.SGSTAmt = Convert.ToDecimal(Result2["SGSTAmt"]);
                    objInvoice.CGSTPer = Convert.ToDecimal(Result2["CGSTPer"]);
                    objInvoice.CGSTAmt = Convert.ToDecimal(Result2["CGSTAmt"]);
                    objInvoice.Total = Convert.ToDecimal(Result2["Total"]);
                    objInvoice.OperationId = Convert.ToInt32(Result2["OperationId"]);

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
                Result2.Close();
                Result2.Dispose();
            }
        }


        public void AddEditInvoiceIRR(PpgIrrInvoice Inv, int BranchId, int userid, int InvoiceId = 0)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = Inv.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Inv.InvoiceDate).ToString("yyyy-MM-dd HH:mm:ss") });


            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = Inv.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = Inv.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = Inv.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = Inv.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GstNo", MySqlDbType = MySqlDbType.VarChar, Value = Inv.GstNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Address", MySqlDbType = MySqlDbType.VarChar, Value = Inv.Address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StateCode", MySqlDbType = MySqlDbType.VarChar, Value = Inv.StateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StateName", MySqlDbType = MySqlDbType.VarChar, Value = Inv.StateName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = Inv.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = Inv.CargoType });

            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationId", MySqlDbType = MySqlDbType.Int32, Value = Inv.OperationId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Quantity", MySqlDbType = MySqlDbType.Decimal, Value = Inv.Quantity });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Rate", MySqlDbType = MySqlDbType.Int32, Value = Inv.Rate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SACCode", MySqlDbType = MySqlDbType.VarChar, Value = Inv.SACCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = Inv.Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Inv.CGSTPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CGST", MySqlDbType = MySqlDbType.Decimal, Value = Inv.CGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Inv.SGSTPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SGST", MySqlDbType = MySqlDbType.Decimal, Value = Inv.SGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = Inv.IGSTPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IGST", MySqlDbType = MySqlDbType.Decimal, Value = Inv.IGST });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Total", MySqlDbType = MySqlDbType.Decimal, Value = Inv.Total });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundOff", MySqlDbType = MySqlDbType.Decimal, Value = Inv.RoundOff });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = Inv.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = Inv.Remarks });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Userid", MySqlDbType = MySqlDbType.Int32, Value = userid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });


            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceIRR", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "IRR Invoice Saved Successfully";
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

        #endregion

        #region Empty Container Payment Sheet

        public void AddEditEmptyContPaymentSheet(VRN_InvoiceYard ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML, string ChargesBreakupXML,
            int BranchId, int Uid,
           string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
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
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditEmptyContPaymentSheet", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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
                    _DBResponse.Status = 3;
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
        public void GetApplicationForEmptyContainer(string ApplicationFor, int ApplicationId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ApplicationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ApplicationFor });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetApplicationForEmptyContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaySheetStuffingRequest> objPaySheetStuffing = new List<PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    if (ApplicationFor == "YARD")
                    {
                        objPaySheetStuffing.Add(new PaySheetStuffingRequest()
                        {
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CHAName = Convert.ToString(Result["CHAName"]),
                            CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                            StuffingReqId = Convert.ToInt32(Result["CustomAppraisementId"]),
                            StuffingReqNo = Convert.ToString(Result["AppraisementNo"]),
                            StuffingReqDate = Convert.ToString(Result["AppraisementDate"]),
                            Address = Convert.ToString(Result["Address"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            State = Convert.ToString(Result["StateName"]),
                            StateCode = Convert.ToString(Result["StateCode"])
                        });
                    }
                    else
                    {
                        objPaySheetStuffing.Add(new PaySheetStuffingRequest()
                        {
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CHAName = Convert.ToString(Result["CHAName"]),
                            CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                            StuffingReqId = Convert.ToInt32(Result["DestuffingId"]),
                            StuffingReqNo = Convert.ToString(Result["DestuffingNo"]),
                            StuffingReqDate = Convert.ToString(Result["DestuffingDate"]),
                            Address = Convert.ToString(Result["Address"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            State = Convert.ToString(Result["StateName"]),
                            StateCode = Convert.ToString(Result["StateCode"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetEmptyContainerListForInvoice()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = DateTime.Now.ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "GateInId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEmptyContainerListForInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<EmptyContainerListForInvoice> objListForInvoice = new List<EmptyContainerListForInvoice>();
            try
            {
                //ShippingLineId, ShippingLineName, GSTNo, CFSCode, ContainerNo, EmptyDate, Address, StateCode, StateName
                while (Result.Read())
                {
                    Status = 1;

                    objListForInvoice.Add(new EmptyContainerListForInvoice()
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        EmptyDate = Convert.ToString(Result["EmptyDate"]),
                        Address = Convert.ToString(Result["Address"]),
                        StateName = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
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
        public void GetEmptyContForPaymentSheet(string ApplicationFor, int ApplicationId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ApplicationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ApplicationFor });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetApplicationForEmptyContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaymentSheetContainer> objPaymentSheetContainer = new List<PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Selected = false,
                        ArrivalDt = Convert.ToString(Result["ArrivalDt"]),
                        IsHaz = Convert.ToString(Result["IsHaz"]),
                        Size = Convert.ToString(Result["Size"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheetContainer;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetEmptyContByEntryId(int EntryId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = DateTime.Now.ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "GateInId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEmptyContainerListForInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaymentSheetContainer> objPaymentSheetContainer = new List<PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Selected = true,
                        ArrivalDt = Convert.ToString(Result["ArrivalDt"]),
                        IsHaz = Convert.ToString(Result["IsHaz"]),
                        Size = Convert.ToString(Result["Size"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheetContainer;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetEmptyContPaymentSheet(string InvoiceDate, int DestuffingAppId, string InvoiceType, string ContainerXML, int InvoiceId, string InvoiceFor, int PartyId, int PayeeId, string ExportUnder)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 20, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceFor", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceFor });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 20, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Size = 20, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            VRN_InvoiceYard objInvoice = new VRN_InvoiceYard();
            DataSet ds = DataAccess.ExecuteDataSet("GetEmptyContPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();

                // while (Result.Read())
                foreach (DataRow Result in ds.Tables[0].Rows)
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }

                foreach (DataRow Result in ds.Tables[1].Rows)
                {
                    objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                    objInvoice.CHAName = Result["CHAName"].ToString();
                    objInvoice.PartyName = Result["CHAName"].ToString();
                    objInvoice.PartyGST = Result["GSTNo"].ToString();
                    objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                    objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                    objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                    objInvoice.PartyAddress = Result["Address"].ToString();
                    objInvoice.PartyStateCode = Result["StateCode"].ToString();

                }

                foreach (DataRow Result in ds.Tables[2].Rows)
                {
                    objInvoice.lstPrePaymentCont.Add(new VRN_PreInvoiceContainer
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ArrivalDate = Result["ArrivalDate"].ToString(),
                        ArrivalTime = Result["ArrivalTime"].ToString(),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        BOENo = Result["BOENo"].ToString(),
                        GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                        WtPerPack = Convert.ToDecimal(Result["WtPerPack"]),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                        CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                        Duty = Convert.ToDecimal(Result["Duty"]),
                        Reefer = Convert.ToInt32(Result["Reefer"]),
                        RMS = Convert.ToInt32(Result["RMS"]),
                        DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                        Insured = Convert.ToInt32(Result["Insured"]),
                        Size = Result["Size"].ToString(),
                        SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                        HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                        AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                        LCLFCL = Result["LCLFCL"].ToString(),
                        CartingDate = Result["CartingDate"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        ApproveOn = Result["ApproveOn"].ToString(),
                        BOEDate = Result["BOEDate"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ImporterExporter = Result["ImporterExporter"].ToString(),
                        LineNo = Result["LineNo"].ToString(),
                        OperationType = Convert.ToInt32(Result["OperationType"]),
                        ShippingLineName = Result["ShippingLineName"].ToString(),
                        SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                    });
                    objInvoice.lstPostPaymentCont.Add(new VRN_PostPaymentContainer
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ArrivalDate = Result["ArrivalDate"].ToString(),
                        ArrivalTime = Result["ArrivalTime"].ToString(),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        BOENo = Result["BOENo"].ToString(),
                        GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                        WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                        CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                        Duty = Convert.ToDecimal(Result["Duty"]),
                        Reefer = Convert.ToInt32(Result["Reefer"]),
                        RMS = Convert.ToInt32(Result["RMS"]),
                        DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                        Insured = Convert.ToInt32(Result["Insured"]),
                        Size = Result["Size"].ToString(),
                        SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                        HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                        AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                        LCLFCL = Result["LCLFCL"].ToString(),
                        CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                        DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                        StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                    });
                }

                foreach (DataRow Result in ds.Tables[3].Rows)
                {
                    objInvoice.lstPostPaymentChrg.Add(new VRN_PostPaymentChrg
                    {
                        ChargeId = Convert.ToInt32(Result["ChargeId"]),
                        Clause = Result["Clause"].ToString(),
                        ChargeType = Result["ChargeType"].ToString(),
                        ChargeName = Result["ChargeName"].ToString(),
                        SACCode = Result["SACCode"].ToString(),
                        Quantity = Convert.ToInt32(Result["Quantity"]),
                        Rate = Convert.ToDecimal(Result["Rate"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        Discount = Convert.ToDecimal(Result["Discount"]),
                        Taxable = Convert.ToDecimal(Result["Taxable"]),
                        IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                        IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                        SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                        SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                        CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                        CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                        Total = Convert.ToDecimal(Result["Total"]),
                        OperationId = Convert.ToInt32(Result["OperationId"]),
                    });
                }

                foreach (DataRow Result in ds.Tables[4].Rows)
                {
                    objInvoice.lstContWiseAmount.Add(new VRN_ContainerWiseAmount
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                        CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                        GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                        GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                        ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                        StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                        InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                        PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                        WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                        ContainerId = 0,
                        InvoiceId = 0,
                        LineNo = ""
                    });
                }

                foreach (DataRow Result in ds.Tables[5].Rows)
                {
                    objInvoice.lstOperationCFSCodeWiseAmount.Add(new VRN_OperationCFSCodeWiseAmount
                    {
                        InvoiceId = InvoiceId,
                        CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        OperationId = Convert.ToInt32(Result["OperationId"]),
                        ChargeType = Result["ChargeType"].ToString(),
                        Quantity = Convert.ToDecimal(Result["Quantity"]),
                        Rate = Convert.ToDecimal(Result["Rate"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        Clause = Convert.ToString(Result["Clause"]),
                    });
                }

                //foreach (DataRow Result in ds.Tables[6].Rows)
                //{
                //    objInvoice.lstPostPaymentChrgBreakup.Add(new VRN_PostPaymentChargebreakupdate
                //    {
                //        ChargeId = Convert.ToInt32(Result["OperationId"]),
                //        Clause = Result["ChargeType"].ToString(),
                //        ChargeType = Result["ChargeType"].ToString(),
                //        ChargeName = Result["ChargeName"].ToString(),
                //        SACCode = Result["SACCode"].ToString(),
                //        Quantity = Convert.ToInt32(Result["Quantity"]),
                //        Rate = Convert.ToDecimal(Result["Rate"]),
                //        Amount = Convert.ToDecimal(Result["Amount"]),
                //        OperationId = Convert.ToInt32(Result["OperationId"]),
                //        CFSCode = Result["CFSCode"].ToString(),
                //        Startdate = Result["StartDate"].ToString(),
                //        EndDate = Result["EndDate"].ToString(),

                //    });
                //}

                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

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
                ds.Dispose();
            }
        }

        public void GetPaymentPartyForImportInvoice()
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
        public void GetBOLForEmptyCont(string InvoiceFor, int DestuffingId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceFor });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingId });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetBOLForEmpCont", CommandType.StoredProcedure, dparam);
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            var BOL = "";
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    BOL = result["BOLNo"].ToString() + ":" + result["BOLDate"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Data = BOL;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = BOL;
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

        #region Job Order By Road

        public void AddEditJobOrderByRoad(PPG_JobOrderByRoad objJO, int BranchId, int CreatedBy, string FormOneDetailXML = null)
        {
            string GeneratedClientId = "0";
            string Status = "0";
            string ReturnObj = "";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Value = objJO.FormOneId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneDate", MySqlDbType = MySqlDbType.Date, Value = objJO.FormOneDate.ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objJO.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneDetailXML", MySqlDbType = MySqlDbType.Text, Value = FormOneDetailXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CONTCBT", MySqlDbType = MySqlDbType.Text, Value = objJO.CONTCBT });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.Text, Value = objJO.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportBy", MySqlDbType = MySqlDbType.Text, Value = objJO.TransportBy });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = CreatedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "@GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "" });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("AddEditJobOrderByRoad", CommandType.StoredProcedure, DParam, out Status, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "JobOrder By Road Saved Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "JobOrder By Road Updated Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Form-1 Details As It Already Exists In Another Page";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = ReturnObj;
                    _DBResponse.Data = null;
                }
                else if (Result == -1)
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

        public void GetJobOrderByRoadList()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetJobOrderByRoad", CommandType.StoredProcedure, DParam);
            IList<PPG_ImportJobOrderByRoadList> lstFormOne = new List<PPG_ImportJobOrderByRoadList>();
            int Status = 0;

            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new PPG_ImportJobOrderByRoadList
                    {
                        FormOneId = Convert.ToInt32(result["FormOneId"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        ShippingLineName = Convert.ToString(result["EximTraderName"]),
                        CONTCBT = Convert.ToString(result["CONTCBT"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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

        public void GetJobOrderByRoadList(string ContainerNo)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo.TrimStart() });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("SearchListJobOrderByRoad", CommandType.StoredProcedure, DParam);
            IList<PPG_ImportJobOrderByRoadList> lstFormOne = new List<PPG_ImportJobOrderByRoadList>();
            int Status = 0;

            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new PPG_ImportJobOrderByRoadList
                    {
                        FormOneId = Convert.ToInt32(result["FormOneId"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        ShippingLineName = Convert.ToString(result["EximTraderName"]),
                        CONTCBT = Convert.ToString(result["CONTCBT"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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
        public void GetJobOrderByRoadId(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetJobOrderByRoad", CommandType.StoredProcedure, DParam);
            PPG_JobOrderByRoad lstFormOne = new PPG_JobOrderByRoad();
            int Status = 0;

            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.FormOneId = Convert.ToInt32(result["FormOneId"]);
                    lstFormOne.FormOneNo = Convert.ToString(result["FormOneNo"]);
                    lstFormOne.FormOneDate = Convert.ToDateTime(result["FormOneDate"]);
                    lstFormOne.ShippingLineId = Convert.ToInt32(result["ShippingLineId"]);
                    lstFormOne.ShippingLineName = Convert.ToString(result["EximTraderName"]);
                    lstFormOne.TransportBy = Convert.ToString(result["TransportBy"]);
                    lstFormOne.CONTCBT = Convert.ToString(result["CONTCBT"]);
                    lstFormOne.Remarks = Convert.ToString(result["Remarks"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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
        public void GetJobOrderByRoadByOnEditMode(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetJobOrderByRoad", CommandType.StoredProcedure, DParam);
            PPG_JobOrderByRoad objFormOne = new PPG_JobOrderByRoad();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objFormOne.FormOneId = Convert.ToInt32(result["FormOneId"]);
                    objFormOne.FormOneNo = Convert.ToString(result["FormOneNo"]);
                    objFormOne.FormOneDate = Convert.ToDateTime(result["FormOneDate"]);
                    objFormOne.ShippingLineId = Convert.ToInt32(result["ShippingLineId"]);
                    objFormOne.ShippingLineName = Convert.ToString(result["EximTraderName"]);
                    objFormOne.CONTCBT = Convert.ToString(result["CONTCBT"]);
                    objFormOne.Remarks = Convert.ToString(result["Remarks"]);
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objFormOne.lstFormOneDetail.Add(new PPG_ImportJobOrderByRoadDtl()
                        {
                            FormOneDetailID = Convert.ToInt32(result["FormOneDetailId"]),
                            ContainerNo = Convert.ToString(result["ContainerNo"]),
                            ContainerSize = Convert.ToString(result["ContainerSize"]),
                            LCLFCL = Convert.ToString(result["LCLFCL"]),
                            ShippingLineId = Convert.ToInt32(result["ShippingLineId"]),
                            ShippingLineName = Convert.ToString(result["EximTraderName"]),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objFormOne.lstFormOneDetail;
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


        public void GetDeliveryAppforMerge(int DeliId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DeliId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetDeliveryApplicationForMerge", CommandType.StoredProcedure, DParam);
            ppgdeliverydet objFormOne = new ppgdeliverydet();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objFormOne.CHAId = Convert.ToInt32(result["CHAId"]);
                    objFormOne.CHAName = Convert.ToString(result["CHAName"]);
                    objFormOne.GSTNo = Convert.ToString(result["GSTNo"]);
                    objFormOne.CFSCode = Convert.ToString(result["CFSCode"]);
                    objFormOne.LineNo = Convert.ToString(result["LineNo"]);
                    objFormOne.BOENo = Convert.ToString(result["BOENo"]);
                    objFormOne.StateCode = Convert.ToString(result["StateCode"]);
                    objFormOne.Address = Convert.ToString(result["Address"]);

                    objFormOne.StateName = Convert.ToString(result["StateName"]);
                    objFormOne.CargoType = Convert.ToInt32(result["CargoType"]);
                    objFormOne.DeliveryAppDate = Convert.ToString(result["DeliveryAppDate"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Data = objFormOne;
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
        public void DeleteJobOrderByRoad(int FormOneId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int Result = DataAccess.ExecuteNonQuery("DeleteFormOne", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Job Order By Road Data Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Job Order By Road Data Does Not Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Job Order By Road Details As It Already Exists In Another Page";
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
        public void JobOrderPrintDetails(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("JobOrderPrintDetails", CommandType.StoredProcedure, DParam);
            List<dynamic> lstFormOne = new List<dynamic>();
            int Status = 0;

            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new
                    {
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        TransportBy = Convert.ToString(result["TransportBy"]),
                        ShippingLineName = Convert.ToString(result["ShippingLine"]),
                        CONTCBT = Convert.ToString(result["CONTCBT"]),
                        ContainerNo = result["ContainerNo"].ToString(),
                        ContainerSize = result["ContainerSize"].ToString(),
                        LCLFCL = result["LCLFCL"].ToString(),
                        ShippingLine = result["ShippingLine"].ToString(),
                        Remarks = result["Remarks"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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

        #region OBL Entry
        public void GetContainerListForOBL(string Type, string CFSCode = "")
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.String, Value = CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.String, Value = Type });

            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLContainerList", CommandType.StoredProcedure, DParam);
            List<VRN_ContainerInfo> LstContainerInfo = new List<VRN_ContainerInfo>();
            string size = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    if (CFSCode == "" || CFSCode == null)
                    {
                        LstContainerInfo.Add(new VRN_ContainerInfo
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            MovementType = Convert.ToString(Result["ContainerLoadType"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingLineName = Convert.ToString(Result["ShippingLine"])
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

        public void GetCFSCodeForOBL(string ContainerNo, string ContainerSize, string CFSCode)
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

        public void GetCFSCodeWiseOBLDetails(string CFSCode, string ContainerNo, string ContainerSize, string IGM_No, string IGM_Date, int OBLEntryId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_CFSCode", MySqlDbType = MySqlDbType.String, Value = CFSCode });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CONTAINER_NO", MySqlDbType = MySqlDbType.String, Value = ContainerNo });

                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLEntryId", MySqlDbType = MySqlDbType.Int32, Value = OBLEntryId });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOblEntryDetails", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                VRN_OblEntry objOBLEntry = new VRN_OblEntry();

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
                        VRN_OblEntryDetails objOBLEntryDetails = new VRN_OblEntryDetails();
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

        public void AddEditOBLEntryLCL(VRN_OblEntry objOBL, string XmlText)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.String, Value = objOBL.ContainerNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CONTCBT", MySqlDbType = MySqlDbType.String, Value = objOBL.CONTCBT });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.String, Value = objOBL.ContainerSize }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.String, Value = objOBL.CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_No", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.IGM_No });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_Date", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.IGM_Date != null ? Convert.ToDateTime(objOBL.IGM_Date).ToString("yyyy-MM-dd") : null });// Value = Convert.ToDateTime(objOBL.IGM_Date).ToString("yyyy-MM-dd") });
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
            int Result = DA.ExecuteNonQuery("AddEditOBLEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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

        public void GetAllOblEntryLCL(int Page)
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
                Result = DataAccess.ExecuteDataSet("GetAllOblEntryForPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<VRN_OblEntry> OblEntryList = new List<VRN_OblEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        VRN_OblEntry objOBLEntry = new VRN_OblEntry();
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

        public void GetOblEntryLCLDetailsByOblEntryId(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetOblEntryDetailsByOblEntryId", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            VRN_OblEntry objOBLEntry = new VRN_OblEntry();
            IList<VRN_OblEntryDetails> lstOBLEntryDetails = new List<VRN_OblEntryDetails>();
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
                        lstOBLEntryDetails.Add(new VRN_OblEntryDetails
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
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            AreaCBM = Convert.ToDecimal(Result["AreaCBM"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
                            TSANo = Convert.ToString(Result["TSANo"]),
                            TSA_Date = Convert.ToString(Result["TSA_Date"])
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
        public void DeleteOBLEntryLCL(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("DeleteOBLEntry", CommandType.StoredProcedure, DParam);
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

        #endregion

        #region OBL Wise Container
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
                Result = DataAccess.ExecuteDataSet("GetListOfOBLWiseContainerSearch", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<VRN_OBLWiseContainerEntry> OblEntryList = new List<VRN_OBLWiseContainerEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        VRN_OBLWiseContainerEntry objOBLEntry = new VRN_OBLWiseContainerEntry();
                        objOBLEntry.impobldtlId = Convert.ToInt32(dr["impobldtlId"]);
                        objOBLEntry.OBL_No = Convert.ToString(dr["OBL_No"]);
                        objOBLEntry.OBL_Date = Convert.ToString(dr["OBL_Date"]);
                        objOBLEntry.IGM_No = Convert.ToString(dr["IGM_No"].ToString());
                        objOBLEntry.IGM_Date = Convert.ToString(dr["IGM_Date"]);
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
        public void GetAllPort()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstPort", CommandType.StoredProcedure, DParam);
            List<PPGPost> LstPort = new List<PPGPost>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new PPGPost
                    {
                        PortName = Result["PortName"].ToString(),
                        PortAlias = Result["PortAlias"].ToString(),
                        PortId = Convert.ToInt32(Result["PortId"]),
                        CountryId = Convert.ToInt32(Result["CountryId"]),
                        StateId = Convert.ToInt32(Result["StateId"]),
                        POD = Convert.ToBoolean(Result["POD"]),
                        CountryName = Result["CountryName"].ToString(),
                        StateName = Result["StateName"].ToString()
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
        public void GetOBLWiseContainerDetails(string OBLNo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLNo", MySqlDbType = MySqlDbType.String, Value = OBLNo });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLWiseContainerDetails", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                VRN_OBLWiseContainerEntry objOBLEntry = new VRN_OBLWiseContainerEntry();


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
                        VRN_OBLWiseContainerEntryDetails objOBLEntryDetails = new VRN_OBLWiseContainerEntryDetails();
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

        public void AddEditOBLWiseContainerEntry(VRN_OBLWiseContainerEntry objOBL, string XmlText)
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.ImporterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DtlXml", MySqlDbType = MySqlDbType.Text, Value = XmlText });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.CommodityId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditOBLWiseContDet", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("ListOfOBLWiseContainer", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<VRN_OBLWiseContainerEntry> OblEntryList = new List<VRN_OBLWiseContainerEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        VRN_OBLWiseContainerEntry objOBLEntry = new VRN_OBLWiseContainerEntry();
                        objOBLEntry.impobldtlId = Convert.ToInt32(dr["impobldtlId"]);
                        objOBLEntry.OBL_No = Convert.ToString(dr["OBL_No"]);
                        objOBLEntry.OBL_Date = Convert.ToString(dr["OBL_Date"]);
                        objOBLEntry.IGM_No = Convert.ToString(dr["IGM_No"].ToString());
                        objOBLEntry.IGM_Date = Convert.ToString(dr["IGM_Date"]);
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
            IList<Areas.Import.Models.ImporterForPage> lstImporter = new List<Areas.Import.Models.ImporterForPage>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstImporter.Add(new Areas.Import.Models.ImporterForPage
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
        public void GetOBLWiseContainerDetailsByID(int impobldtlId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_impobldtlId", MySqlDbType = MySqlDbType.Int32, Value = impobldtlId });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLWiseContainerDetailsByID", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                VRN_OBLWiseContainerEntry objOBLEntry = new VRN_OBLWiseContainerEntry();

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
                        objOBLEntry.PortName = Convert.ToString(Result.Tables[0].Rows[0]["PortName"]);
                        objOBLEntry.CountryId = Convert.ToInt32(Result.Tables[0].Rows[0]["CountryId"]);
                        objOBLEntry.CountryName = Convert.ToString(Result.Tables[0].Rows[0]["CountryName"]);
                        objOBLEntry.LineNo = Convert.ToString(Result.Tables[0].Rows[0]["LINE_NO"].ToString());
                        objOBLEntry.CargoDescription = Convert.ToString(Result.Tables[0].Rows[0]["CargoDescription"]);
                        objOBLEntry.NoOfPkg = Convert.ToString(Result.Tables[0].Rows[0]["NO_PKG"]);
                        objOBLEntry.PkgType = Convert.ToString(Result.Tables[0].Rows[0]["PKG_TYPE"]);
                        objOBLEntry.GR_WT = Convert.ToDecimal(Result.Tables[0].Rows[0]["GR_WT"]);
                        objOBLEntry.ImporterId = Convert.ToInt32(Result.Tables[0].Rows[0]["ImporterId"]);
                        objOBLEntry.ImporterName = Convert.ToString(Result.Tables[0].Rows[0]["ImporterName"]);
                        objOBLEntry.CargoType = Convert.ToInt32(Result.Tables[0].Rows[0]["CargoType"]);
                        objOBLEntry.SMTPNo = Convert.ToString(Result.Tables[0].Rows[0]["SMTPNo"]);
                        objOBLEntry.SMTP_Date = Convert.ToString(Result.Tables[0].Rows[0]["SMTP_Date"]);
                        objOBLEntry.CommodityId = Convert.ToInt32(Result.Tables[0].Rows[0]["CommodityId"]);
                        objOBLEntry.Commodity = Convert.ToString(Result.Tables[0].Rows[0]["Commodity"]);
                    }

                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        VRN_OBLWiseContainerEntryDetails objOBLEntryDetails = new VRN_OBLWiseContainerEntryDetails();
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
        #endregion

        #region OBL Amendment

        public void ListOfOBLNo(string OBLNo, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLNo", MySqlDbType = MySqlDbType.VarChar, Value = OBLNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLContainerForPage", CommandType.StoredProcedure, Dparam);
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
        public void OBLAmendmentDetail(string OBLNo)
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
                Result = DataAccess.ExecuteDataSet("GetOBLAmendmentDetail", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();
                OBLAmendment objOBLEntry = new OBLAmendment();
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(Result.Tables[0].Rows[0]["RetValue"]) == 1)
                        {
                            objOBLEntry.RetValue = Convert.ToInt32(Result.Tables[0].Rows[0]["RetValue"]);
                        }
                        else
                        {
                            objOBLEntry.OldOBLNo = Convert.ToString(Result.Tables[0].Rows[0]["OldOBLNo"]);
                            objOBLEntry.OBLDate = Convert.ToString(Result.Tables[0].Rows[0]["OBLDate"]);
                            objOBLEntry.IGMNo = Convert.ToString(Result.Tables[0].Rows[0]["IGMNo"]);
                            objOBLEntry.IGMDate = Convert.ToString(Result.Tables[0].Rows[0]["IGMDate"]);
                            objOBLEntry.CFSCode = Convert.ToString(Result.Tables[0].Rows[0]["CFSCode"]);
                            objOBLEntry.ContainerNo = Convert.ToString(Result.Tables[0].Rows[0]["ContainerNo"]);
                            objOBLEntry.OldNoOfPkg = Convert.ToInt32(Result.Tables[0].Rows[0]["OldNoOfPkg"]);
                            objOBLEntry.OldGRWT = Convert.ToDecimal(Result.Tables[0].Rows[0]["OldGRWT"]);
                            objOBLEntry.RetValue = Convert.ToInt32(Result.Tables[0].Rows[0]["RetValue"]);
                        }
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

        public void AddEditOBLAmendment(OBLAmendment objOBL)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OldOBLNo", MySqlDbType = MySqlDbType.String, Value = objOBL.OldOBLNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OldNoPKG", MySqlDbType = MySqlDbType.String, Value = objOBL.OldNoOfPkg });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OldGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objOBL.OldGRWT) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NewOBLNo", MySqlDbType = MySqlDbType.String, Value = objOBL.NewOBLNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NewNoPKG", MySqlDbType = MySqlDbType.String, Value = objOBL.NewNoOfPkg });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NewGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = objOBL.NewGRWT });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditOBLAmendment", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "OBL No Updated Successfully";
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLNoForOBLSearch", CommandType.StoredProcedure, Dparam);
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
        public void GetICEGateDetail(string OBLNo, string SearchBy)
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
                LstParam.Add(new MySqlParameter { ParameterName = "In_SearchBy", MySqlDbType = MySqlDbType.String, Value = SearchBy });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetICEGateData", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                OBLWiseContainerEntry objOBLEntry = new OBLWiseContainerEntry();

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
                        OBLWiseContainerEntryDetails objOBLEntryDetails = new OBLWiseContainerEntryDetails();
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

        #region Empty Container Shipping Line transfer
        public void GetEmptyContaierListForTransfer()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetEmptyContainerListForTransfer", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            List<dynamic> lstInvData = new List<dynamic>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstInvData.Add(new {
                        ShippingLineId = Convert.ToInt32(result["ShippingLineId"]),
                        ShippingLineName = result["ShippingLineName"].ToString(),
                        GSTNo = result["GSTNo"].ToString(),
                        EntryId = Convert.ToInt32(result["EntryId"]),
                        CFSCode = result["CFSCode"].ToString(),
                        ContainerNo = result["ContainerNo"].ToString(),
                        Size = result["Size"].ToString(),
                        EntryDate = result["EntryDate"].ToString(),
                        EmptyDate = result["EmptyDate"].ToString(),
                        Address = result["Address"].ToString(),
                        StateCode = result["StateCode"].ToString(),
                        StateName = result["StateName"].ToString(),
                        RefId = Convert.ToInt32(result["RefId"]),
                        RefNo = result["RefNo"].ToString(),
                        RefDate = result["RefDate"].ToString(),
                        // ToShippingLineId = result["ToShippingId"].ToString(),
                        // ToShippingLineName = result["ToShippingLineName"].ToString(),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstInvData;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "No data";
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

        public void GetEmptyContToShipLineForTransfer(string ContainerNo)
        {
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEmptyContToShipLineForTransfer", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            List<dynamic> lstInvData = new List<dynamic>();
            int Status = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInvData.Add(new
                    {

                        ToShippingLineId = Result["ToShippingId"].ToString(),
                        ToShippingLineName = Result["ToShippingLineName"].ToString(),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstInvData;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "No data";
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

        public void CalculateEmptyContTransferInv(string InvoiceDate, string InvoiceType, string CFSCode, string ContainerNo, string Size, string EntryDate,
            string EmptyDate, int RefId, int PartyId, int PayeeId, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(EntryDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EmptyDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(EmptyDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RefId", MySqlDbType = MySqlDbType.Int32, Value = RefId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            PpgInvoiceYard objInvoice = new PpgInvoiceYard();
            DataSet ds = DataAccess.ExecuteDataSet("GetEmptyContTransferPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();
                foreach (DataRow Result in ds.Tables[0].Rows)
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();
                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();
                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();
                }
                foreach (DataRow Result in ds.Tables[1].Rows)
                {
                    objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                    objInvoice.CHAName = Result["CHAName"].ToString();
                    objInvoice.PartyName = Result["CHAName"].ToString();
                    objInvoice.PartyGST = Result["GSTNo"].ToString();
                    objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                    objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                    objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                    objInvoice.PartyAddress = Result["Address"].ToString();
                    objInvoice.PartyStateCode = Result["StateCode"].ToString();
                }
                foreach (DataRow Result in ds.Tables[2].Rows)
                {
                    objInvoice.lstPrePaymentCont.Add(new PpgPreInvoiceContainer
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ArrivalDate = Result["ArrivalDate"].ToString(),
                        ArrivalTime = Result["ArrivalTime"].ToString(),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        BOENo = Result["BOENo"].ToString(),
                        GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                        WtPerPack = Convert.ToDecimal(Result["WtPerPack"]),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                        CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                        Duty = Convert.ToDecimal(Result["Duty"]),
                        Reefer = Convert.ToInt32(Result["Reefer"]),
                        RMS = Convert.ToInt32(Result["RMS"]),
                        DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                        Insured = Convert.ToInt32(Result["Insured"]),
                        Size = Result["Size"].ToString(),
                        SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                        HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                        AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                        LCLFCL = Result["LCLFCL"].ToString(),
                        CartingDate = Result["CartingDate"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        ApproveOn = Result["ApproveOn"].ToString(),
                        BOEDate = Result["BOEDate"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ImporterExporter = Result["ImporterExporter"].ToString(),
                        LineNo = Result["LineNo"].ToString(),
                        OperationType = Convert.ToInt32(Result["OperationType"]),
                        ShippingLineName = Result["ShippingLineName"].ToString(),
                        SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                    });
                    objInvoice.lstPostPaymentCont.Add(new PpgPostPaymentContainer
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ArrivalDate = Result["ArrivalDate"].ToString(),
                        ArrivalTime = Result["ArrivalTime"].ToString(),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        BOENo = Result["BOENo"].ToString(),
                        GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                        WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                        CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                        Duty = Convert.ToDecimal(Result["Duty"]),
                        Reefer = Convert.ToInt32(Result["Reefer"]),
                        RMS = Convert.ToInt32(Result["RMS"]),
                        DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                        Insured = Convert.ToInt32(Result["Insured"]),
                        Size = Result["Size"].ToString(),
                        SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                        HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                        AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                        LCLFCL = Result["LCLFCL"].ToString(),
                        CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                        DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                        StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                    });
                }
                foreach (DataRow Result in ds.Tables[3].Rows)
                {
                    objInvoice.lstPostPaymentChrg.Add(new PpgPostPaymentChrg
                    {
                        ChargeId = Convert.ToInt32(Result["ChargeId"]),
                        Clause = Result["ChargeType"].ToString(),
                        ChargeType = Result["ChargeType"].ToString(),
                        ChargeName = Result["ChargeName"].ToString(),
                        SACCode = Result["SACCode"].ToString(),
                        Quantity = Convert.ToInt32(Result["Quantity"]),
                        Rate = Convert.ToDecimal(Result["Rate"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        Discount = Convert.ToDecimal(Result["Discount"]),
                        Taxable = Convert.ToDecimal(Result["Taxable"]),
                        IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                        IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                        SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                        SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                        CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                        CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                        Total = Convert.ToDecimal(Result["Total"]),
                        OperationId = Convert.ToInt32(Result["OperationId"]),
                    });
                }
                foreach (DataRow Result in ds.Tables[4].Rows)
                {
                    objInvoice.lstContWiseAmount.Add(new PpgContainerWiseAmount
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                        CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                        GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                        GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                        ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                        StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                        InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                        PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                        WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                        ContainerId = 0,
                        InvoiceId = 0,
                        LineNo = ""
                    });
                }
                foreach (DataRow Result in ds.Tables[5].Rows)
                {
                    objInvoice.lstOperationCFSCodeWiseAmount.Add(new PpgOperationCFSCodeWiseAmount
                    {
                        InvoiceId = InvoiceId,
                        CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        OperationId = Convert.ToInt32(Result["OperationId"]),
                        ChargeType = Result["ChargeType"].ToString(),
                        Quantity = Convert.ToDecimal(Result["Quantity"]),
                        Rate = Convert.ToDecimal(Result["Rate"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        //Clause = Convert.ToString(Result["Clause"]),
                    });
                }


                foreach (DataRow Result in ds.Tables[6].Rows)
                {
                    objInvoice.lstPostPaymentChrgBreakup.Add(new ppgPostPaymentChargebreakupdate
                    {
                        ChargeId = Convert.ToInt32(Result["OperationId"]),
                        Clause = Result["ChargeType"].ToString(),
                        ChargeType = Result["ChargeType"].ToString(),
                        ChargeName = Result["ChargeName"].ToString(),
                        SACCode = Result["SACCode"].ToString(),
                        Quantity = Convert.ToInt32(Result["Quantity"]),
                        Rate = Convert.ToDecimal(Result["Rate"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        OperationId = Convert.ToInt32(Result["OperationId"]),
                        CFSCode = Result["CFSCode"].ToString(),
                        Startdate = Result["StartDate"].ToString(),
                        EndDate = Result["EndDate"].ToString(),
                        //Clause = Convert.ToString(Result["Clause"]),
                    });
                }


                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

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
                ds.Dispose();
            }
        }


        public void AddEditEmptyContTransferPaymentSheet(PpgInvoiceYard ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML, string ChargesBreakupXML,
         int BranchId, int Uid,
        string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            if (ObjPostPaymentSheet.RequestDate == "")
            {
                ObjPostPaymentSheet.RequestDate = "1900-01-01";
            }
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToShippingId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToShippingName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.LocationName });

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
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditEmptyContTransferPaymentSheet", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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
                    _DBResponse.Status = 3;
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

        #region Resend IceGate Data
        public void ResendIceGateData(string ContainerNo)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("ResendIceGateData", CommandType.StoredProcedure, Dparam);

            _DBResponse = new DatabaseResponse();
            try
            {

                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Resend IceGate Data Successful";
                    _DBResponse.Data = Result;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Fail To Resend IceGate Data";
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

        #region merge app

        public void AddEditMergeDeliveryApplication(MergeDeliveryIssueViewModel ObjDeliveryApp, string DeliveryXml, String ObjDeliveryOrd)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.DeliveryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryXML", MySqlDbType = MySqlDbType.Text, Value = DeliveryXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrdXML", MySqlDbType = MySqlDbType.Text, Value = ObjDeliveryOrd });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = ObjDeliveryApp.DeliApp.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjDeliveryApp.DeliApp.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.OTHr });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output, Value = ReturnObj });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            VRN_mergedet pdet = new VRN_mergedet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMergeDeliveryApplication", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    pdet.Id = Convert.ToInt32(GeneratedClientId);
                    pdet.AppNo = ReturnObj;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Delivery Application Details  Saved Successfully";
                    _DBResponse.Data = pdet;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Delivery Application Details Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Invoice cannot be generated due to sd balance is low";
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

        public void AddEditSingleMergeDeliveryApplication(AMD_MergeDeliveryIssueViewModel ObjDeliveryApp,
           string DeliveryAppDtlXml, string DeliveryOrdDtlXml,
           string lstPrePaymentContXML, string lstPostPaymentContXML,
                 string lstPostPaymentChrgXML, string lstContWiseAmountXML,
                 string lstOperationCFSCodeWiseAmountXML, string lstPostPaymentChrgBreakupXML, string lstInvoiceCargoXML, string DeliveryGodownDtlXml)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.DeliveryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.DestuffingId });


            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryXML", MySqlDbType = MySqlDbType.Text, Value = DeliveryAppDtlXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrdXML", MySqlDbType = MySqlDbType.Text, Value = DeliveryOrdDtlXml });


            LstParam.Add(new MySqlParameter { ParameterName = "in_lstPostPaymentChrgXML", MySqlDbType = MySqlDbType.Text, Value = lstPostPaymentChrgXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_lstContWiseAmountXML", MySqlDbType = MySqlDbType.Text, Value = lstContWiseAmountXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryGodownDtlXml", MySqlDbType = MySqlDbType.Text, Value = DeliveryGodownDtlXml });


            LstParam.Add(new MySqlParameter { ParameterName = "in_lstOperationCFSCodeWiseAmountXML", MySqlDbType = MySqlDbType.Text, Value = lstOperationCFSCodeWiseAmountXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_lstPostPaymentChrgBreakupXML", MySqlDbType = MySqlDbType.Text, Value = lstPostPaymentChrgBreakupXML });

            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjDeliveryApp.DeliApp.DeliveryDate).ToString("yyyy-MM-dd HH:mm:ss") });// ObjDeliveryApp.DeliApp.DeliveryDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjDeliveryApp.DeliApp.InvoiceDate).ToString("yyyy-MM-dd HH:mm:ss") });// ObjDeliveryApp.DeliApp.DeliveryDate });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = ObjDeliveryApp.DeliApp.PartyName });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = ObjDeliveryApp.DeliApp.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjDeliveryApp.DeliApp.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.OTHr });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNos", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.VehicleNos });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNoUn", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.VehicleNoUn });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDeliveryApp.DeliApp.SEZ) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Insured", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDeliveryApp.DeliApp.IsInsured) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OwnMovement", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDeliveryApp.DeliApp.IsTransporter) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BillToParty", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDeliveryApp.DeliApp.BillToParty) });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToDecimal(ObjDeliveryApp.RoundUp) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ImpToBondConversion", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryApp.ConvertToBondFlag });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.ImporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjDeliveryApp.DeliApp.ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomExam", MySqlDbType = MySqlDbType.VarChar, Value = ObjDeliveryApp.CstmExam });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortDistance", MySqlDbType = MySqlDbType.Decimal, Value = ObjDeliveryApp.PortDistanc });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output, Value = ReturnObj });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            WFLDmergedet pdet = new WFLDmergedet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMergeDelPaymentIssue", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    pdet.Id = Convert.ToInt32(GeneratedClientId);
                    pdet.AppNo = ReturnObj;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Delivery Application Details with Invoice and Issue Slip Saved Successfully";
                    _DBResponse.Data = pdet;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Delivery Application Details Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Invoice cannot be generated due to  balance is low";
                    _DBResponse.Data = null;
                }

                else if (Result == 4)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "0 Amount Invoice can not be generated";
                    _DBResponse.Data = null;
                }

                else if (Result == 5)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Invoice already generated";
                    _DBResponse.Data = null;
                }

                else if (Result == 6)
                {
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Invoice Date should be Greater than or equal to already generated Invoice Date";
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


        //public void AddEditSingleMergeDeliveryApplication(VRN_MergeDeliveryIssueViewModel ObjDeliveryApp, string DeliveryXml, String ObjDeliveryOrd)
        //{
        //    string GeneratedClientId = "0";
        //    string ReturnObj = "";


        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.DeliveryId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.DestuffingId });

        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Movement", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.Movement });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = ObjDeliveryApp.Distance });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_IsBond", MySqlDbType = MySqlDbType.Int16, Value = ObjDeliveryApp.ConvertToBond });

        //    LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryXML", MySqlDbType = MySqlDbType.Text, Value = DeliveryXml });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrdXML", MySqlDbType = MySqlDbType.Text, Value = ObjDeliveryOrd });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = ObjDeliveryApp.DeliApp.DeliveryDate });

        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.CHAId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.PartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = ObjDeliveryApp.DeliApp.PartyName });

        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.PayeeId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = ObjDeliveryApp.DeliApp.PayeeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjDeliveryApp.DeliApp.InvoiceType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.OTHr });
        //    // LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.ImporterId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });
        //    LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
        //    LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output, Value = ReturnObj });

        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    VRN_mergedet pdet = new VRN_mergedet();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    int Result = DataAccess.ExecuteNonQuery("AddEditMergeDelPaymentIssue", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        if (Result == 1)
        //        {
        //            pdet.Id = Convert.ToInt32(GeneratedClientId);
        //            pdet.AppNo = ReturnObj;
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Delivery Application Details with Invoice and Issue Slip Saved Successfully";
        //            _DBResponse.Data = pdet;
        //        }
        //        else if (Result == 2)
        //        {
        //            _DBResponse.Status = 2;
        //            _DBResponse.Message = "Delivery Application Details Updated Successfully";
        //            _DBResponse.Data = null;
        //        }
        //        else if (Result == 3)
        //        {
        //            _DBResponse.Status = 3;
        //            _DBResponse.Message = "Invoice cannot be generated due to sd balance is low";
        //            _DBResponse.Data = null;
        //        }

        //        else if (Result == 4)
        //        {
        //            _DBResponse.Status = 3;
        //            _DBResponse.Message = "0 Amount Invoice can not be generated";
        //            _DBResponse.Data = null;
        //        }

        //        else if (Result == 5)
        //        {
        //            _DBResponse.Status = 3;
        //            _DBResponse.Message = "Invoice already generated";
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


        public void GetAllDeliveryMergeApplication(int Page, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMergeDelivaryAppForPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VRN_MergeDeliveryApplicationList> LstDeliveryApp = new List<VRN_MergeDeliveryApplicationList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDeliveryApp.Add(new VRN_MergeDeliveryApplicationList
                    {
                        DeliveryId = Convert.ToInt32(Result["DeliveryId"].ToString()),
                        DeliveryNo = Result["DeliveryNo"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        DeliveryDate = Result["InvoiceDate"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        PartyID = Convert.ToInt32(Result["PartyId"]),
                        IssueSlipId = Convert.ToInt32(Result["IssueSlipId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDeliveryApp;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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



        #region SingleClick DeliveryPaymentSheet
        public void GetDeliveryPaymentSheetSingle(string InvoiceType, int DestuffingAppId, string DeliveryDate, string ExportUnder, string CustomExam, int InvoiceId, string XMLText, int PartyId, int PayeeId, 
            int IsFranchise, int IsOnWheel, int IsReworking, int IsCargoShifting, int IsDestuffing, int IsCargoDelivery, int IsLiftOnOff,int IsSweeping, int IsHandling,
            int OTHours = 0, int OblFlag = 0, int Movement = 0, int Distance = 0, int IsBond = 0, int PortDistance = 0 )
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryXML", MySqlDbType = MySqlDbType.Text, Value = XMLText });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = DeliveryDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Value = OTHours });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OblFlag", MySqlDbType = MySqlDbType.Int32, Value = OblFlag });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Movement", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Movement });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsBond", MySqlDbType = MySqlDbType.Decimal, Value = IsBond });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomExam", MySqlDbType = MySqlDbType.VarChar, Value = CustomExam });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortDistance", MySqlDbType = MySqlDbType.Decimal, Value = PortDistance });

            LstParam.Add(new MySqlParameter { ParameterName = "in_IsFranchise", MySqlDbType = MySqlDbType.Decimal, Value = IsFranchise });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsOnWheel", MySqlDbType = MySqlDbType.Decimal, Value = IsOnWheel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsReworking", MySqlDbType = MySqlDbType.Decimal, Value = IsReworking });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsCargoShifting", MySqlDbType = MySqlDbType.Decimal, Value = IsCargoShifting });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsDestuffing", MySqlDbType = MySqlDbType.Decimal, Value = IsDestuffing });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsCargoDelivery", MySqlDbType = MySqlDbType.Decimal, Value = IsCargoDelivery });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsLiftOnOff", MySqlDbType = MySqlDbType.Decimal, Value = IsLiftOnOff });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsSweeping", MySqlDbType = MySqlDbType.Decimal, Value = IsSweeping });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsHandling", MySqlDbType = MySqlDbType.Decimal, Value = IsHandling });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();


            VRN_InvoiceGodown objInvoice = new VRN_InvoiceGodown();
            IDataReader Result = DataAccess.ExecuteDataReader("getsingleclickdeliverypaymentsheet", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["DestuffingId"]);
                        objInvoice.RequestNo = Result["DestuffingNo"].ToString();
                        objInvoice.RequestDate = Result["DestuffingDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();

                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPrePaymentCont.Add(new VRN_PreInvoiceContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWeight = Result["GrossWeight"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerPack = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Result["CIFValue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Result["DelDuty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["DelDuty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Result["RMS"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Result["DeliveryType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Result["Insured"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Result["HeavyScrap"] == DBNull.Value ? 0 : Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Result["AppraisementPerct"] == DBNull.Value ? 0 : Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = " ",
                            CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ApproveOn = "0",
                            BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            OperationType = 0,
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new VRN_PostPaymentContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["DelDuty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Result["RMS"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Result["DeliveryType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Result["Insured"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Result["HeavyScrap"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = " ",
                            CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContWiseAmount.Add(new VRN_ContainerWiseAmount
                        {
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            EntryFee = Result["EntryFee"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Result["CstmRevenue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Result["GrEmpty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Result["GrLoaded"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Result["ReeferCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Result["StorageCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Result["InsuranceCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Result["PortCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Result["WeighmentCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WeighmentCharge"]),
                            CustomExamCharge = Result["CustomExamCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CustomExamCharge"]),
                            ContainerId = 0,
                            InvoiceId = 0,
                            LineNo = ""
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstInvoiceCargo.Add(new VRN_InvoiceCargo
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
                            GodownWiseLctnNames = Result["GodownWiseLctnNames"].ToString(),
                            GodownWiseLocationIds = Result["GodownWiseLocationIds"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new VRN_PostPaymentChrg
                        {
                            ChargeId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"] == System.DBNull.Value ? "" : Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                            Discount = Result["Discount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Discount"]),
                            Taxable = Result["Taxable"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Result["IGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Result["IGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Result["SGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Result["SGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Result["CGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Result["CGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Result["Total"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Total"]),
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                        });
                    }


                }

                if (Result.NextResult())
                {
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new VRN_OperationCFSCodeWiseAmount
                        {
                            InvoiceId = InvoiceId,
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"] == System.DBNull.Value ? "" : Result["ContainerNo"].ToString(),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    if (Result.Read())
                    {
                        objInvoice.hasSDAvailableBalance = Convert.ToInt32(Result["hasSDAvailableBalance"]);
                    }
                }
                if (Result.NextResult())
                {
                    if (Result.Read())
                    {
                        objInvoice.PaymentMode = Result["IN_MODE"].ToString();
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        objInvoice.lstPostPaymentChrgBreakup.Add(new VRN_DeliPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            Startdate = Result["StartDate"].ToString(),
                            EndDate = Result["EndDate"].ToString(),

                        });
                    }


                }
                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

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


        public void GetGSTValue(int PartyId)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });

            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGSTforSClick", CommandType.StoredProcedure, Dparam);
            String GSTNo = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;


                    GSTNo = Result["GSTNO"].ToString();

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = GSTNo;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region List Of ExpInvoice
        public void ListOfImpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofexpInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<VRN_ListOfImpInvoice> lstExpInvoice = new List<VRN_ListOfImpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new VRN_ListOfImpInvoice()
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

        #region ImportBondConversion Godown
        public void GetAllOBLNo()
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            IDataParameter[] Dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetOBLForBondConversion", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            List<OBLNoForBondConversion> lstOBL = new List<OBLNoForBondConversion>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstOBL.Add(new OBLNoForBondConversion
                    {
                        OBLNo = result["OBLNo"].ToString(),
                        DestuffingId = Convert.ToInt32(result["DestuffingEntryDtlId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstOBL;
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


        public void GetGodwonById(int DestuffingId)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingId });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetGodownForBondTransfer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            ImportBondConversion objConv = new ImportBondConversion();
            try
            {


                if (Result.Read())
                {

                    Status = 1;

                    objConv.FromGodownId = Convert.ToInt32(Result["GodownId"]);
                    objConv.FromGodownName = Convert.ToString(Result["GodownName"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Data = objConv;
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

        public void GetLocationForBondTransfer(int DestuffingId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLocationForBondTransfer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LocationForBondTransfer> LstLocation = new List<LocationForBondTransfer>();
            try
            {
                while (Result.Read())

                {
                    Status = 1;
                    LstLocation.Add(new LocationForBondTransfer
                    {
                        FromLocationId = Convert.ToInt32(Result["LoactionId"]),
                        FromLocationName = Convert.ToString(Result["LocationName"]),
                        DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"])

                    });
                    if (Status == 1)
                    {
                        _DBResponse.Status = 1;
                        _DBResponse.Message = "Success";
                        _DBResponse.Data = LstLocation;
                    }
                    else
                    {
                        _DBResponse.Status = 0;
                        _DBResponse.Message = "No Data";
                        _DBResponse.Data = null;
                    }
                }
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

        public void GetOBLDetailsByDestuffingId(int DestuffingEntryId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingEntryId });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLDetForBondTransfer", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                ImportBondConversion objImportBondConversion = new ImportBondConversion();

                if (Result != null && Result.Tables.Count > 0)
                {
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        Status = 1;
                        objImportBondConversion.FromGodownId = Convert.ToInt32(Result.Tables[0].Rows[0]["GodownId"]);
                        objImportBondConversion.FromGodownName = Convert.ToString(Result.Tables[0].Rows[0]["GodownName"]);
                        objImportBondConversion.FromLocationId = Convert.ToString(Result.Tables[0].Rows[0]["FromLocationId"]);
                        objImportBondConversion.FromLocationName = Convert.ToString(Result.Tables[0].Rows[0]["LocationNames"]);
                        objImportBondConversion.NoOfPkg = Convert.ToInt32(Result.Tables[0].Rows[0]["NoOfPkg"]);
                        objImportBondConversion.Weight = Convert.ToDecimal(Result.Tables[0].Rows[0]["GrWeight"]);
                        objImportBondConversion.SQM = Convert.ToDecimal(Result.Tables[0].Rows[0]["SQM"]);
                        objImportBondConversion.CUM = Convert.ToDecimal(Result.Tables[0].Rows[0]["CUM"]);
                        objImportBondConversion.CIF = Convert.ToDecimal(Result.Tables[0].Rows[0]["CIF"]);
                        objImportBondConversion.Duty = Convert.ToDecimal(Result.Tables[0].Rows[0]["Duty"]);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objImportBondConversion;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetAllSacNo(string SAC = "")
        {
            String ReturnObj = "";
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = SAC });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = lstparam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetSacForBondConversion", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;

            List<ImportBondConversion> LstSac = new List<ImportBondConversion>();
            try
            {
                while (Result.Read())

                {
                    Status = 1;
                    LstSac.Add(new ImportBondConversion
                    {
                        SACId = Convert.ToInt32(Result["SacId"]),
                        SACNo = Convert.ToString(Result["SACNo"]),
                        SACDate = Result["SACDate"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSac;
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

        public void GetAllInternalBondMovement(string Area)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Area", MySqlDbType = MySqlDbType.VarChar, Value = Area });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ImportBondConversion> LstInternalMovement = new List<ImportBondConversion>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new ImportBondConversion
                    {
                        MovementNo = Result["MovementNo"].ToString(),
                        MovementDate = Result["MovementDate"].ToString(),
                        OBLNo = Result["BOENo"].ToString(),
                        MovementId = Convert.ToInt32(Result["MovementId"]),
                        OBLDate = Result["BOEDate"].ToString(),
                        GodownName = Result["GodownName"].ToString()
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
        public void GetAllInternalBondMovementyardSearch(string Area, string obl)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_obl", MySqlDbType = MySqlDbType.VarChar, Value = obl });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Area", MySqlDbType = MySqlDbType.VarChar, Value = Area });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpInternalMovementSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ImportBondConversion> LstInternalMovement = new List<ImportBondConversion>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new ImportBondConversion
                    {
                        MovementNo = Result["MovementNo"].ToString(),
                        MovementDate = Result["MovementDate"].ToString(),
                        OBLNo = Result["BOENo"].ToString(),
                        MovementId = Convert.ToInt32(Result["MovementId"]),
                        OBLDate = Result["BOEDate"].ToString(),
                        GodownName = Result["GodownName"].ToString()
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

        public void GetAllInternalBondMovementSearch(string Area, string obl)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_obl", MySqlDbType = MySqlDbType.VarChar, Value = obl });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Area", MySqlDbType = MySqlDbType.VarChar, Value = Area });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpInternalMovementSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ImportBondConversion> LstInternalMovement = new List<ImportBondConversion>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new ImportBondConversion
                    {
                        MovementNo = Result["MovementNo"].ToString(),
                        MovementDate = Result["MovementDate"].ToString(),
                        OBLNo = Result["BOENo"].ToString(),
                        MovementId = Convert.ToInt32(Result["MovementId"]),
                        OBLDate = Result["BOEDate"].ToString(),
                        GodownName = Result["GodownName"].ToString()
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

        public void AddEditImportBondConversion(ImportBondConversion ObjInternalMovement)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.MovementDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.OBLNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.OBLDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.NoOfPkg });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.FromGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.FromLocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.FromLocationName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.Location });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ModuleNameMovement", MySqlDbType = MySqlDbType.VarChar, Value = "Import" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacId", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.SACId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SACNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.SACNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SACDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.SACDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Value", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.CIF });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Duty", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.Duty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SQM", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.SQM });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });

            LstParam.Add(new MySqlParameter { ParameterName = "in_WrNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.WRNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WrDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.WRDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.BondNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.BondDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomBondNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.CustomBondNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomBondDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.CustomBondDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.CustomSealNo });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditImportBondConversion", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjInternalMovement.MovementId == 0 ? "Internal Movement Details Saved Successfully" : "Internal Movement  Details Updated Successfully";
                    _DBResponse.Data = null;
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

        public void GetBondInternalMovement(int MovementId, string Area)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Area", MySqlDbType = MySqlDbType.VarChar, Value = Area });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            ImportBondConversion ObjInternalMovement = new ImportBondConversion();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjInternalMovement.MovementId = Convert.ToInt32(Result["MovementId"]);
                    ObjInternalMovement.MovementNo = Result["MovementNo"].ToString();
                    ObjInternalMovement.MovementDate = Result["MovementDate"].ToString();
                    ObjInternalMovement.OBLNo = Result["OBLNo"].ToString();
                    ObjInternalMovement.OBLDate = Result["OBLDate"].ToString();
                    ObjInternalMovement.FromGodownId = Convert.ToInt32(Result["FromGodownId"] == DBNull.Value ? 0 : Result["FromGodownId"]);
                    ObjInternalMovement.FromGodownName = Result["FromGodownName"].ToString();
                    ObjInternalMovement.FromLocationId = Result["FromLocationId"].ToString();
                    ObjInternalMovement.FromLocationName = Result["FromLocationName"].ToString();
                    ObjInternalMovement.NoOfPkg = Convert.ToInt32(Result["NoOfPkg"].ToString());
                    ObjInternalMovement.Weight = Convert.ToDecimal(Result["Weight"]);
                    ObjInternalMovement.SQM = Convert.ToDecimal(Result["SQM"]);
                    ObjInternalMovement.CUM = Convert.ToDecimal(Result["CUM"]);
                    ObjInternalMovement.CIF = Convert.ToDecimal(Result["CIF"]);
                    ObjInternalMovement.Duty = Convert.ToDecimal(Result["Duty"]);
                    ObjInternalMovement.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjInternalMovement.GodownName = Result["GodownName"].ToString();
                    ObjInternalMovement.SACId = Convert.ToInt32(Result["SACId"]);
                    ObjInternalMovement.SACNo = Result["SACNo"].ToString();
                    ObjInternalMovement.SACDate = Result["SACDate"].ToString();
                    ObjInternalMovement.DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]);
                    ObjInternalMovement.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjInternalMovement.LocationId = Result["LocationId"].ToString();
                    ObjInternalMovement.Location = Result["Location"].ToString();
                    ObjInternalMovement.WRNo = Result["WRNo"].ToString();
                    ObjInternalMovement.WRDate = Result["WRDate"].ToString();
                    ObjInternalMovement.BondNo = Result["BondNo"].ToString();
                    ObjInternalMovement.BondDate = Result["BondDate"].ToString();
                    ObjInternalMovement.CustomBondNo = Result["CustomBondNo"].ToString();
                    ObjInternalMovement.CustomBondDate = Result["CustomBondDate"].ToString();
                    ObjInternalMovement.CustomSealNo = Result["CustomSealNo"].ToString();
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


        #region Import Bond Conversion Yard
        public void GetAllOBLNoForYard()
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            IDataParameter[] Dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetOBLForBondConvYard", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            List<OBLNoForBondConversion> lstOBL = new List<OBLNoForBondConversion>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstOBL.Add(new OBLNoForBondConversion
                    {
                        OBLNo = result["OBLNo"].ToString(),
                        DestuffingId = Convert.ToInt32(result["DestuffingEntryDtlId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstOBL;
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
        public void GetAppraisementDetById(int DestuffingEntryId, string OBLNo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingEntryId });
                LstParam.Add(new MySqlParameter { ParameterName = "in_OBLNo", MySqlDbType = MySqlDbType.VarChar, Value = OBLNo });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLDetForBondTrnsYard", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                ImportBondConversion objImportBondConversion = new ImportBondConversion();

                if (Result != null && Result.Tables.Count > 0)
                {
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        Status = 1;
                        objImportBondConversion.FromGodownId = Convert.ToInt32(Result.Tables[0].Rows[0]["GodownId"]);
                        objImportBondConversion.FromGodownName = Convert.ToString(Result.Tables[0].Rows[0]["GodownName"]);
                        objImportBondConversion.FromLocationId = Convert.ToString(Result.Tables[0].Rows[0]["FromLocationId"]);
                        objImportBondConversion.FromLocationName = Convert.ToString(Result.Tables[0].Rows[0]["LocationNames"]);
                        objImportBondConversion.NoOfPkg = Convert.ToInt32(Result.Tables[0].Rows[0]["NoOfPkg"]);
                        objImportBondConversion.Weight = Convert.ToDecimal(Result.Tables[0].Rows[0]["GrWeight"]);
                        objImportBondConversion.SQM = Convert.ToDecimal(Result.Tables[0].Rows[0]["SQM"]);
                        objImportBondConversion.CUM = Convert.ToDecimal(Result.Tables[0].Rows[0]["CUM"]);
                        objImportBondConversion.CIF = Convert.ToDecimal(Result.Tables[0].Rows[0]["CIF"]);
                        objImportBondConversion.Duty = Convert.ToDecimal(Result.Tables[0].Rows[0]["Duty"]);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objImportBondConversion;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void AddEditImportBondConversionYard(ImportBondConversion ObjInternalMovement)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.MovementDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.OBLNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.OBLDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.NoOfPkg });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.FromGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.FromLocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.FromLocationName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.Location });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ModuleNameMovement", MySqlDbType = MySqlDbType.VarChar, Value = "Import" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacId", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.SACId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SACNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.SACNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SACDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.SACDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Value", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.CIF });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Duty", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.Duty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SQM", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.SQM });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });

            LstParam.Add(new MySqlParameter { ParameterName = "in_WrNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.WRNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WrDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.WRDate == null ? ObjInternalMovement.WRDate : Convert.ToDateTime(ObjInternalMovement.WRDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.BondNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.BondDate == null ? ObjInternalMovement.BondDate : Convert.ToDateTime(ObjInternalMovement.BondDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomBondNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.CustomBondNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomBondDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.CustomBondDate == null ? ObjInternalMovement.CustomBondDate : Convert.ToDateTime(ObjInternalMovement.CustomBondDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.CustomSealNo });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditImportBondConversionYard", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjInternalMovement.MovementId == 0 ? "Internal Movement Details Saved Successfully" : "Internal Movement  Details Updated Successfully";
                    _DBResponse.Data = null;
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

        public void GetAllGodown(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllGodownExp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Areas.Import.Models.Godown> LstGodown = new List<Areas.Import.Models.Godown>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstGodown.Add(new Areas.Import.Models.Godown
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

        
    }
}

