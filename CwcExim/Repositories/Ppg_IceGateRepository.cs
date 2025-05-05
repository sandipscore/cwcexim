using CwcExim.Areas.Icegate.Models;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Dynamic;
using CwcExim.Models;

namespace CwcExim.Repositories
{
    public class Ppg_IceGateRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        #region Ice gate report
        public void EximtraderlistPopulation(int Page, string PartyCode, int Exporter = 0, int Importer = 0, int ShippingLine = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Value = PartyCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Exporter", MySqlDbType = MySqlDbType.Int32, Value = Exporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLine", MySqlDbType = MySqlDbType.Int32, Value = ShippingLine });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Importer", MySqlDbType = MySqlDbType.Int32, Value = Importer });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            DataSet Result = new DataSet();
            Result = DataAccess.ExecuteDataSet("IcegateEximtraderlist", CommandType.StoredProcedure, Dparam);

            List<dynamic> LstParty = new List<dynamic>();
            bool State = false;
            _DBResponse = new DatabaseResponse();
            try
            {
                Status = 1;
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    LstParty.Add(new { EximTraderId = Convert.ToInt32(dr["EximTraderId"]), EximTraderName = dr["EximTraderName"].ToString(), EximTraderAlias = dr["EximTraderAlias"].ToString() });
                }
                if (Result.Tables[1].Rows.Count > 0)
                {
                    State = Convert.ToBoolean(Result.Tables[1].Rows[0][0]);
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }
        public void GetContainerList(char Module)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = Module });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            DataSet Result = new DataSet();
            Result = DataAccess.ExecuteDataSet("IcegateContCFSCodeList", CommandType.StoredProcedure, Dparam);
            List<dynamic> LstCont = new List<dynamic>();
            _DBResponse = new DatabaseResponse();
            try
            {
                Status = 1;
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    LstCont.Add(new { ContainerNo = Convert.ToString(dr["ContainerNo"]), CFSCode = dr["CFSCode"].ToString() });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCont;
                }
                else
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
            }
        }
        public void IcegateI02Report(string FromDate, string ToDate, string Mode, string CFSCode = "", string OBL = "", int ShippingId = 0, int ImpExpId = 0)
        {
            int Status = 0;
            FromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            ToDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = ToDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = 'I' });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Mode", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = Mode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBLSB", MySqlDbType = MySqlDbType.VarChar, Value = OBL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingId", MySqlDbType = MySqlDbType.Int32, Value = ShippingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImpExpId", MySqlDbType = MySqlDbType.Int32,  Value = ImpExpId });

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            DataSet Result = new DataSet();
            Result = DataAccess.ExecuteDataSet("IcegateI02E07Report", CommandType.StoredProcedure, Dparam);
            var LstCont = new List<IcegateI02Model>();
            var LstContAck = new List<IcegateI02ModelAck>();
            _DBResponse = new DatabaseResponse();
            try
            {
                if(Result.Tables[0].Rows.Count>0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        LstCont.Add(new IcegateI02Model()
                        {
                            ContainerNo = dr["ContainerNo"].ToString(),
                            CFSCode = dr["CFSCode"].ToString(),
                            FileCode = dr["FileCode"].ToString(),
                            FileName = dr["FileName"].ToString(),
                            SendOn = dr["SendOn"].ToString().Split(' ')[0],
                            OBLNo = dr["OBLSB_No"].ToString(),
                            Importer = dr["ImpExpName"].ToString(),
                            SLA = dr["SLAName"].ToString(),
                            AckStatus = dr["AckStatus"].ToString(),
                            DateofMsg = dr["DateofMsg"].ToString().Split(' ')[0]
                        });
                    }
                }
                
                foreach (DataRow dr in Result.Tables[1].Rows)
                {
                    LstContAck.Add(new IcegateI02ModelAck()
                    {
                        
                        FileCode = dr["FileCode"].ToString(),
                        FileName = dr["FileName"].ToString(),
                        ErrorCode = dr["ErrorCode"].ToString(),
                        AckRecvDate = dr["AckRecvDate"].ToString().Split(' ')[0],
                    });
                }
                if (LstCont.Count > 0)
                    Status = 1;
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data =new { LstCont, LstContAck };
                }
                else
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
            }
        }
        public void IcegateE07Report(string FromDate, string ToDate, string Mode, string CFSCode = "", string OBL = "", int ShippingId = 0, int ImpExpId = 0)
        {
            int Status = 0;
            FromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            ToDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = ToDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = 'E' });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Mode", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = Mode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBLSB", MySqlDbType = MySqlDbType.VarChar, Value = OBL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingId", MySqlDbType = MySqlDbType.Int32, Value = ShippingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImpExpId", MySqlDbType = MySqlDbType.Int32, Value = ImpExpId });

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            DataSet Result = new DataSet();
            Result = DataAccess.ExecuteDataSet("IcegateI02E07Reportv2", CommandType.StoredProcedure, Dparam);
            var LstCont = new List<IcegateE07Model>();
            var LstContAck = new List<IcegateE07ModelAck>();
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        LstCont.Add(new IcegateE07Model()
                        {
                            ContainerNo = dr["ContainerNo"].ToString(),
                            CFSCode = dr["CFSCode"].ToString(),
                            FileCode = dr["FileCode"].ToString(),
                            FileName = dr["FileName"].ToString(),
                            SendOn = dr["SendOn"].ToString().Split(' ')[0],
                            OBLNo = dr["OBLSB_No"].ToString(),
                            Importer = dr["ImpExpName"].ToString(),
                            SLA = dr["SLAName"].ToString(),
                            AckStatus = dr["AckStatus"].ToString(),
                            AckRecvDate = dr["AckRecvDate"].ToString(),

                            DateofMsg = dr["DateofMsg"].ToString().Split(' ')[0]
                        });
                    }
                }

                foreach (DataRow dr in Result.Tables[1].Rows)
                {
                    LstContAck.Add(new IcegateE07ModelAck()
                    {

                        FileCode = dr["FileCode"].ToString(),
                        FileName = dr["FileName"].ToString(),
                        ErrorCode = dr["ErrorCode"].ToString(),
                        AckRecvDate = dr["AckRecvDate"].ToString().Split(' ')[0],
                    });
                }
                if (LstCont.Count > 0)
                    Status = 1;
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { LstCont, LstContAck };
                }
                else
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
            }
        }
        public void SaveResentRecord(string FileName, string FileCode, string ContainerNo, string CFSCode,string Module)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FileName", MySqlDbType = MySqlDbType.VarChar, Value = FileName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FileCode", MySqlDbType = MySqlDbType.VarChar, Value = FileCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0 ,Direction=ParameterDirection.Output});
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("AddResentI02E07Record", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            try
            {
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
        }
        #endregion

        #region Manual I02
        public void GetContainerListForI02()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = "" });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            DataSet Result = new DataSet();
            Result = DataAccess.ExecuteDataSet("GetContDetForImpManual", CommandType.StoredProcedure, Dparam);
            List<dynamic> LstCont = new List<dynamic>();
            _DBResponse = new DatabaseResponse();
            try
            {
                Status = 1;
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    LstCont.Add(new { ContainerNo = Convert.ToString(dr["ContainerNo"]), CFSCode = dr["CFSCode"].ToString() });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCont;
                }
                else
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
            }
        }
        public void GetContainerDtlForI02(string CFSCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = CFSCode });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            DataSet Result = new DataSet();
            Result = DataAccess.ExecuteDataSet("GetContDetForImpManual", CommandType.StoredProcedure, Dparam);
            //dynamic Cont=new { ContainerNo="", CFSCode="", Size ="", SLA ="", TransportMode ="", Train_VehicleDate ="", Train_VehicleNo ="", WagonNo ="", Weight =0.00};
            dynamic Cont = new ExpandoObject();
            var lstTP = new List<dynamic>();
            _DBResponse = new DatabaseResponse();
            try
            {
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    Status = 1;
                    Cont.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                    Cont.CFSCode = Convert.ToString(dr["CFSCode"]);
                    Cont.Size = Convert.ToString(dr["Size"]);
                    Cont.SLA = Convert.ToString(dr["SLA"]);
                    Cont.TransportMode = Convert.ToString(dr["TransportMode"]);
                    Cont.Train_VehicleDate = Convert.ToString(dr["Train_VehicleDate"]);
                    Cont.ContainerLoadType = Convert.ToString(dr["ContainerLoadType"]);
                    Cont.Train_VehicleNo = Convert.ToString(dr["Train_VehicleNo"]);
                    Cont.WagonNo = Convert.ToString(dr["WagonNo"]);
                    Cont.Weight = Convert.ToDecimal(dr["Weight"]);
                    Cont.DestRailStationCode= Convert.ToString(dr["DestRailStationCode"]);
                    Cont.EntryId = Convert.ToInt32(dr["EntryId"]);
                    Cont.PortName = dr["PortName"].ToString();
                    Cont.CBT = Convert.ToInt32(dr["CBT"]);
                    Cont.PortAlias = dr["GatewayPortCode"].ToString();
                }
                foreach(DataRow dr in Result.Tables[1].Rows)
                {
                    lstTP.Add(new { SMTPNo=dr["TPNo"].ToString() , SMTPDate = dr["TPDate"].ToString() });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { Cont, lstTP };
                }
                else
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
            }
        }
        public void AddManualI02Dtl(string xml = "")
        {
            try
            {
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = xml });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("addmanuali02dtl", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                if(Result==1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Manual I02 created successfully";
                    _DBResponse.Data = Result;
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
        public int GetFileCodeForI02()
        {
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer(); 
            IDataReader result = DataAccess.ExecuteDataReader("Ices_ManualIgmFileCode", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {
                int FileCode = 0;
                while(result.Read())
                {
                    FileCode = Convert.ToInt32(result["FileCode"]);
                }
                return FileCode;
            }
            catch
            {
                return 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void ListManualI02(int id, int page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = page });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            DataSet Result = new DataSet();
            Result = DataAccess.ExecuteDataSet("ListManualI02", CommandType.StoredProcedure, Dparam);
            List<IcegateManualI02> LstCont = new List<IcegateManualI02>();
            _DBResponse = new DatabaseResponse();
            try
            {
                Status = 1;
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    LstCont.Add(new IcegateManualI02
                    {
                        SerialNo = Convert.ToInt32(dr["SerialNo"]),
                        EntryId = Convert.ToInt32(dr["EntryId"]),
                        ContainerNo = Convert.ToString(dr["ContainerNo"]),
                        CFSCode = dr["CFSCode"].ToString(),
                        Train_VehicleNo = dr["Train_VehicleNo"].ToString(),
                        SendOn = dr["SendOn"].ToString(),
                        CarrierAgencyCode = dr["CarrierAgencyCode"].ToString(),
                        FileCode = Convert.ToInt32(dr["FileCode"]),
                        FileName = dr["FileName"].ToString(),
                        ShippingLineCode=dr["ShippingLineCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCont;
                }
                else
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
            }
        }
        public void ViewManualI02FileCreate(int id)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            DataSet Result = new DataSet();
            Result = DataAccess.ExecuteDataSet("ListManualI02", CommandType.StoredProcedure, Dparam);
            
            IcegateManualI02ViewModel objVM = new IcegateManualI02ViewModel();
            _DBResponse = new DatabaseResponse();
            try
            {
                Status = 1;
               
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    objVM.SerialNo = Convert.ToInt32(dr["SerialNo"]);
                    objVM.EntryId = Convert.ToInt32(dr["EntryId"]);
                    objVM.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                    objVM.CFSCode = dr["CFSCode"].ToString();
                    objVM.CBT = Convert.ToInt32(dr["CBT"]);
                    objVM.ModeOfTransport = dr["ModeOfTransport"].ToString();
                    objVM.CustomHouseCode = dr["CustomHouseCode"].ToString();
                    objVM.Train_VehicleNo = dr["Train_VehicleNo"].ToString();
                    objVM.Train_VehicleDate = dr["Train_VehicleDate"].ToString();
                    objVM.WagonNo = dr["WagonNo"].ToString();
                    objVM.PortCodeOrigin = dr["PortCodeOrigin"].ToString();
                    objVM.Weight = Convert.ToDecimal(dr["Weight"]);
                    objVM.DestRailStationCode = dr["DestRailStationCode"].ToString();
                    objVM.GatewayPortCode = dr["GatewayPortCode"].ToString();
                    objVM.BondNo = dr["BondNo"].ToString();
                    objVM.ISOCode = dr["ISOCode"].ToString();
                    objVM.ContainerStatus = dr["ContainerStatus"].ToString();
                    objVM.SendOn = dr["SendOn"].ToString();
                    objVM.CarrierAgencyCode = dr["CarrierAgencyCode"].ToString();
                    objVM.FileCode = Convert.ToInt32(dr["FileCode"]);
                    objVM.FileName = dr["FileName"].ToString();
                    objVM.ShippingLineCode = dr["ShippingLineCode"].ToString();
                    objVM.lstSMTP.Add(new SMTPDet
                    {
                        
                        SMTPNo=dr["SMTPNo"].ToString(),
                        SMTPDate = dr["SMTPDate"].ToString(),
                        
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objVM;
                }
                else
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
            }
        }
        #endregion

        #region Manual E07
        public void GetContainerListForE07()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = "" });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            DataSet Result = new DataSet();
            Result = DataAccess.ExecuteDataSet("GetContDetForExpManualV2", CommandType.StoredProcedure, Dparam);
            List<dynamic> LstCont = new List<dynamic>();
            _DBResponse = new DatabaseResponse();
            try
            {
                Status = 1;
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    LstCont.Add(new
                    {
                        ContainerNo = Convert.ToString(dr["ContainerNo"]),
                        CFSCode = dr["CFSCode"].ToString(),
                        Sbno = dr["Sbno"].ToString(),
                        SbDate = dr["SbDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCont;
                }
                else
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
            }
        }
        public void GetContainerDtlForE07(string CFSCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = CFSCode });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            DataSet Result = new DataSet();
            Result = DataAccess.ExecuteDataSet("GetContDetForExpManualV2", CommandType.StoredProcedure, Dparam);
            dynamic Cont = new ExpandoObject();
            var lstTP = new List<dynamic>();
            _DBResponse = new DatabaseResponse();
            try
            {
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    Status = 1;
                    Cont.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                    Cont.CFSCode = Convert.ToString(dr["CFSCode"]);
                    Cont.Size = Convert.ToString(dr["CarrierAgencyCode"]);
                    Cont.SLA = Convert.ToString(dr["ShippingLineCode"]);
                    Cont.TransportMode = Convert.ToString(dr["ModeOfTransport"]);
                    Cont.Train_VehicleDate = Convert.ToString(dr["Train_VehicleDate"]);
                    Cont.ContainerLoadType = Convert.ToString(dr["ContainerStatus"]);
                    Cont.Train_VehicleNo = Convert.ToString(dr["Train_VehicleNo"]);
                    Cont.WagonNo = Convert.ToString(dr["WagonNo"]);
                    Cont.Weight = Convert.ToDecimal(dr["Weight"]);
                    Cont.DestRailStationCode = Convert.ToString(dr["DestRailStationCode"]);
                    Cont.PortCodeOrigin= dr["PortCodeOrigin"].ToString();
                    Cont.PortAlias = dr["GatewayPortCode"].ToString();
                    Cont.FCLLCL = dr["FCLLCL"].ToString();
                }
                foreach (DataRow dr in Result.Tables[1].Rows)
                {
                    lstTP.Add(new {
                                    SMTPNo = dr["TPNo"].ToString(),
                                    SMTPDate = dr["TPDate"].ToString(),
                                     Weight = (dr["Weight"]).ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { Cont, lstTP };
                }
                else
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
            }
        }
        public void AddManualE07Dtl(string xml = "")
        {
            try
            {
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = xml });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("addmanuale07dtl", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Manual E07 created successfully";
                    _DBResponse.Data = Result;
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
        public int GetFileCodeForE07()
        {
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("Ices_ManualEgmFileCode", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {
                int FileCode = 0;
                while (result.Read())
                {
                    FileCode = Convert.ToInt32(result["FileCode"]);
                }
                return FileCode;
            }
            catch
            {
                return 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void ListManualE07(int id ,int page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = page });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            DataSet Result = new DataSet();
            Result = DataAccess.ExecuteDataSet("ListManualE07", CommandType.StoredProcedure, Dparam);
            List<IcegateManualE07> LstCont = new List<IcegateManualE07>();
            _DBResponse = new DatabaseResponse();
            try
            {
                Status = 1;
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    LstCont.Add(new IcegateManualE07
                    {
                        SerialNo = Convert.ToInt32(dr["SerialNo"]),
                        ContainerNo = Convert.ToString(dr["ContainerNo"]),
                        CFSCode = dr["CFSCode"].ToString(),
                        Train_VehicleNo = dr["Train_VehicleNo"].ToString(),
                        SendOn = dr["SendOn"].ToString(),
                        CarrierAgencyCode=dr["CarrierAgencyCode"].ToString(),
                        FileCode=Convert.ToInt32(dr["FileCode"]),
                        FileName=dr["FileName"].ToString(),
                        ShippingLineCode=dr["ShippingLineCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCont;
                }
                else
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
            }
        }
        public void ViewManualE07FileCreate(int id)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            DataSet Result = new DataSet();
            Result = DataAccess.ExecuteDataSet("ListManualE07", CommandType.StoredProcedure, Dparam);

            IcegateManualE07ViewModel objVM = new IcegateManualE07ViewModel();
            _DBResponse = new DatabaseResponse();
            try
            {
                Status = 1;
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    objVM.SerialNo = Convert.ToInt32(dr["SerialNo"]);
                    objVM.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                    objVM.CFSCode = dr["CFSCode"].ToString();
                    objVM.ModeOfTransport = dr["ModeOfTransport"].ToString();
                    objVM.CustomHouseCode = dr["CustomHouseCode"].ToString();
                    objVM.Train_VehicleNo = dr["Train_VehicleNo"].ToString();
                    objVM.Train_VehicleDate = dr["Train_VehicleDate"].ToString();
                    objVM.WagonNo = dr["WagonNo"].ToString();
                    objVM.PortCodeOrigin = dr["PortCodeOrigin"].ToString();
                    objVM.Weight = Convert.ToDecimal(dr["Weight"]);
                    objVM.DestRailStationCode = dr["DestRailStationCode"].ToString();
                    objVM.GatewayPortCode = dr["GatewayPortCode"].ToString();
                    objVM.BondNo = dr["BondNo"].ToString();
                    objVM.ISOCode = dr["ISOCode"].ToString();
                    objVM.ContainerStatus = dr["ContainerStatus"].ToString();
                    objVM.SendOn = dr["SendOn"].ToString();
                    objVM.CarrierAgencyCode = dr["CarrierAgencyCode"].ToString();
                    objVM.FileCode = Convert.ToInt32(dr["FileCode"]);
                    objVM.FileName = dr["FileName"].ToString();
                    objVM.ShippingLineCode = dr["ShippingLineCode"].ToString();
                    objVM.lstDtl.Add(new SMTPDtl
                    {
                        SMTPNo = dr["SMTPNo"].ToString(),
                        SMTPDate = dr["SMTPDate"].ToString(),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objVM;
                }
                else
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
            }
        }
        #endregion

    }
}