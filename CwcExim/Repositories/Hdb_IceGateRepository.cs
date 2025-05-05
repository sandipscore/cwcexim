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
    public class Hdb_IceGateRepository
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImpExpId", MySqlDbType = MySqlDbType.Int32, Value = ImpExpId });

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            DataSet Result = new DataSet();
            Result = DataAccess.ExecuteDataSet("IcegateI02E07Report", CommandType.StoredProcedure, Dparam);
            var LstCont = new List<IcegateI02Model>();
            var LstContAck = new List<IcegateI02ModelAck>();
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result.Tables[0].Rows.Count > 0)
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
        public void SaveResentRecord(string FileName, string FileCode, string ContainerNo, string CFSCode, string Module)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FileName", MySqlDbType = MySqlDbType.VarChar, Value = FileName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FileCode", MySqlDbType = MySqlDbType.VarChar, Value = FileCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
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
    }

}