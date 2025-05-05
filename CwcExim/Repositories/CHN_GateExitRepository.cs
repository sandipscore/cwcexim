using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Areas.GateOperation.Models;
using Newtonsoft.Json;
using CwcExim.Models;
using CwcExim.DAL;
using CwcExim.UtilityClasses;
using System.Data;
using MySql.Data.MySqlClient;

namespace CwcExim.Repositories
{
    public class CHN_GateExitRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

            }
        }


        #region CBT GateExit
        public void GetTime()
        {
            int Status = 0;
            //List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //IDataParameter[] DParam = { };
            //DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDateTime", CommandType.StoredProcedure, null);
            _DBResponse = new DatabaseResponse();
            CHN_ExitThroughGateHeader objExitThroughGateHeader = new CHN_ExitThroughGateHeader();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objExitThroughGateHeader.GateExitDateTime = Result["CurrentDate"].ToString();
                    objExitThroughGateHeader.Time = Result["CurrentTime"].ToString();


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objExitThroughGateHeader;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void GetCBTList()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCBTListForGateExit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CBTList> lstCBT = new List<CBTList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCBT.Add(new CBTList
                    {
                        CBTNo = Result["CBT"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        Size = Convert.ToString(Result["Size"]),
                        GateInDate = Convert.ToString(Result["GateInDate"]),
                        GateInTime = Convert.ToString(Result["GateInTime"])


                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCBT;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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


        public void AddEditExitThroughGateCBT(CHN_GateExitCBT ObjExitThroughGateHeader, int Uid)
        {
            //    DateTime GatePassDate = DateTime.ParseExact(ObjExitThroughGateHeader.GatePassDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //  DateTime GatePassDate = Convert.ToDateTime(ObjExitThroughGateHeader.GatePassDate).ToString("yyyy-MM-dd");

            //  var Exitdt = DateTime.ParseExact(ObjExitThroughGateHeader.GateExitDateTime, "yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
            //if(ObjExitThroughGateHeader.GatePassId=="")
            //{
            //    ObjExitThroughGateHeader.GatePassId = ""
            //}

            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExitId", MySqlDbType = MySqlDbType.Int32, Value = ObjExitThroughGateHeader.ExitId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateExitDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjExitThroughGateHeader.GateExitDateTime).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CBTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjExitThroughGateHeader.CBTNo == null ? null : ObjExitThroughGateHeader.CBTNo.ToUpper() });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjExitThroughGateHeader.CFSCode });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = ObjExitThroughGateHeader.Size });

            LstParam.Add(new MySqlParameter { ParameterName = "in_DriverName", MySqlDbType = MySqlDbType.VarChar, Value = ObjExitThroughGateHeader.DriverName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Contact", MySqlDbType = MySqlDbType.VarChar, Value = ObjExitThroughGateHeader.ContactNo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditCBTGateExit", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "CBT Exit Through Gate Saved Successfully" : "CBT Exit Through Gate Updated Successfully";
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

                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void GetAllExitThroughGateCBT()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllGateExitCBT", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_GateExitCBT> LstExitThroughGate = new List<CHN_GateExitCBT>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstExitThroughGate.Add(new CHN_GateExitCBT
                    {
                        GateExitNo = Result["GateExitNo"].ToString(),
                        GateExitDateTime = Result["GateExitDateTime"].ToString(),
                        CBTNo = Result["CBTNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ExitId = Convert.ToInt32(Result["ExitId"]),
                        Size = Convert.ToString(Result["Size"]),
                        DriverName = Convert.ToString(Result["DriverName"]),
                        ContactNo = Convert.ToString(Result["ContactNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstExitThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void EditViewExitThroughGateCBT(int ExitId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExitId", MySqlDbType = MySqlDbType.VarChar, Value = ExitId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditGateExitCBT", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_GateExitCBT ObjExitThroughGate = new CHN_GateExitCBT();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjExitThroughGate.GateExitNo = Result["GateExitNo"].ToString();
                    ObjExitThroughGate.GateExitDateTime = Result["GateExitDateTime"].ToString();
                    ObjExitThroughGate.CBTNo = Result["CBTNo"].ToString();
                    ObjExitThroughGate.CFSCode = Result["CFSCode"].ToString();
                    ObjExitThroughGate.ExitId = Convert.ToInt32(Result["ExitId"]);
                    ObjExitThroughGate.Size = Convert.ToString(Result["Size"]);
                    ObjExitThroughGate.DriverName = Convert.ToString(Result["DriverName"]);
                    ObjExitThroughGate.ContactNo = Convert.ToString(Result["ContactNo"]);
                    ObjExitThroughGate.GateInDate = Convert.ToString(Result["GateInDate"]);
                    ObjExitThroughGate.GateInTime = Convert.ToString(Result["GateInTime"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjExitThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void SearchGateExitCBT(string CBTNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CBTNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = CBTNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("SearchGateExitCBT", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_GateExitCBT> LstExitThroughGate = new List<CHN_GateExitCBT>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstExitThroughGate.Add(new CHN_GateExitCBT
                    {
                        GateExitNo = Result["GateExitNo"].ToString(),
                        GateExitDateTime = Result["GateExitDateTime"].ToString(),
                        CBTNo = Result["CBTNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ExitId = Convert.ToInt32(Result["ExitId"]),
                        Size = Convert.ToString(Result["Size"]),
                        DriverName = Convert.ToString(Result["DriverName"]),
                        ContactNo = Convert.ToString(Result["ContactNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstExitThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void DeleteExitThroughGateCBT(int ExitId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExitId", MySqlDbType = MySqlDbType.Int32, Value = ExitId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteExitThroughGateCBT", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Record Of CBT Exit Through Gate Deleted Successfully";
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

    }
}