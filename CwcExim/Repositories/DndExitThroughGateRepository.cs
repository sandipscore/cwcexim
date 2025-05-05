using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using CwcExim.Models;
using System.Data;
using CwcExim.Areas.GateOperation.Models;
using System.Globalization;
using System.IO;
namespace CwcExim.Repositories
{
    public class DndExitThroughGateRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void AddEditExitThroughGate(Dnd_ExitThroughGateHeader ObjExitThroughGateHeader, int Uid)
        {
            //    DateTime GatePassDate = DateTime.ParseExact(ObjExitThroughGateHeader.GatePassDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //  DateTime GatePassDate = Convert.ToDateTime(ObjExitThroughGateHeader.GatePassDate).ToString("yyyy-MM-dd");

            //  var Exitdt = DateTime.ParseExact(ObjExitThroughGateHeader.GateExitDateTime, "yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
            //if(ObjExitThroughGateHeader.GatePassId=="")
            //{
            //    ObjExitThroughGateHeader.GatePassId = ""
            //}

            ObjExitThroughGateHeader.Uid = Uid;
            string GeneratedClientId = "0";
            string ReturnInvoiceNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExitIdHeader", MySqlDbType = MySqlDbType.Int32, Value = ObjExitThroughGateHeader.ExitIdHeader });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateExitNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjExitThroughGateHeader.GateExitNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjExitThroughGateHeader.GatePassNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = ObjExitThroughGateHeader.GatePassId });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.Int32, Value = int.Parse(ObjExitThroughGateHeader.CFSCode) });*/

            LstParam.Add(new MySqlParameter { ParameterName = "in_GateExitDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjExitThroughGateHeader.GateExitDateTime).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjExitThroughGateHeader.GatePassDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ViaId", MySqlDbType = MySqlDbType.Int32, Value = ObjExitThroughGateHeader.ViaId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Via", MySqlDbType = MySqlDbType.VarChar, Value = ObjExitThroughGateHeader.Via });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Value = ObjExitThroughGateHeader.Vessel });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Value = ObjExitThroughGateHeader.PortId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Port", MySqlDbType = MySqlDbType.VarChar, Value = ObjExitThroughGateHeader.PortName });

            LstParam.Add(new MySqlParameter { ParameterName = "in_StrExitThroughGateDetails", MySqlDbType = MySqlDbType.Text, Value = ObjExitThroughGateHeader.StrExitThroughGateDetails });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjExitThroughGateHeader.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjExitThroughGateHeader.BranchId });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ObjExitThroughGateHeader.ExitIdHeader, Direction = ParameterDirection.Output });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditExitThroughGate", CommandType.StoredProcedure, DParam, out GeneratedClientId,out ReturnInvoiceNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = ReturnInvoiceNo;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Exit Through Gate Saved Successfully" : "Exit Through Gate Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = ReturnInvoiceNo;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Gate Exit and Invoice cannot be generated as SD balance is low.";
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = ReturnInvoiceNo;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Gate Exit and Invoice cannot be generated as Party has NO SD Account.";
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



        public void GetContainer()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEnteredContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Dnd_ExitThroughGateHeader objExitThroughGateHeader = new Dnd_ExitThroughGateHeader();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objExitThroughGateHeader.containerList.Add(new Dnd_containerExit
                    {
                        ContainerName = Result["ContainerNo"].ToString(),
                        shippingLine = Result["shippingLine"].ToString(),
                        shippingLineId = Result["shippingLineId"].ToString(),//CFSCode
                        CFSCode = Result["CFSCode"].ToString()

                    });
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


        public void GetGatePassLst()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GatePassList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<GatePassList> LstGatePass = new List<GatePassList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstGatePass.Add(new GatePassList
                    {
                        GatePassId = Convert.ToInt32(Result["GatePassId"].ToString()),
                        GatePassNo = Result["GatePassNo"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstGatePass;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void ExitMailStatus(string containerId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_containerId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = containerId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("ExitMailStatus", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Email Of Entry Through Gate Sent Successfully";
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
            Dnd_ExitThroughGateHeader objExitThroughGateHeader = new Dnd_ExitThroughGateHeader();
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

        public void AddGateEXitToDetails(Dnd_ExitThroughGateHeader ObjExitThroughGateHeader, int Uid)
        {
            ObjExitThroughGateHeader.Uid = Uid;
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExitIdHeader", MySqlDbType = MySqlDbType.Int32, Value = ObjExitThroughGateHeader.ExitIdHeader });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateExitNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjExitThroughGateHeader.GateExitNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjExitThroughGateHeader.GatePassNo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_GateExitDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjExitThroughGateHeader.GateExitDateTime).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjExitThroughGateHeader.GatePassDate).ToString("yyyy-MM-dd") });

            LstParam.Add(new MySqlParameter { ParameterName = "in_AddStrExitThroughGateDetails", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjExitThroughGateHeader.StrExitThroughGateDetails });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjExitThroughGateHeader.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjExitThroughGateHeader.BranchId });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 60, Value = ObjExitThroughGateHeader.ExitIdHeader, Direction = ParameterDirection.Output });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddToExitThroughGateDtls", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Exit Through Gate Saved Successfully";
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

        public void DeleteExitThroughGate(int ExitId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExitId", MySqlDbType = MySqlDbType.Int32, Value = ExitId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteExitThroughGate", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Record Of Exit Through Gate Deleted Successfully";
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

        public void GetAllExitThroughGate()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllExitThroughGateHdr", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_ExitThroughGateHeader> LstExitThroughGate = new List<Dnd_ExitThroughGateHeader>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstExitThroughGate.Add(new Dnd_ExitThroughGateHeader
                    {
                        GateExitNo = Result["GateExitNo"].ToString(),
                        GateExitDateTime = Result["GateExitDateTime"].ToString(),
                        GatePassNo = Result["GatePassNo"].ToString(),
                        ExitIdHeader = Convert.ToInt32(Result["ExitIdHeader"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        VehicleNo = Convert.ToString(Result["VehicleNo"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        Module = Result["Module"].ToString()
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

        public void SearchGateExit(string Value)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Value", MySqlDbType = MySqlDbType.VarChar, Value = Value });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("SearchGateExit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_ExitThroughGateHeader> LstExitThroughGate = new List<Dnd_ExitThroughGateHeader>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstExitThroughGate.Add(new Dnd_ExitThroughGateHeader
                    {
                        GateExitNo = Result["GateExitNo"].ToString(),
                        GateExitDateTime = Result["GateExitDateTime"].ToString(),
                        GatePassNo = Result["GatePassNo"].ToString(),
                        ExitIdHeader = Convert.ToInt32(Result["ExitIdHeader"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        VehicleNo = Convert.ToString(Result["VehicleNo"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"])
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
        public void GetExitThroughGateDetails()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExitIdDtls", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllExitThroughGateDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_ExitThroughGateDetails> LstExitThroughGateDtls = new List<Dnd_ExitThroughGateDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstExitThroughGateDtls.Add(new Dnd_ExitThroughGateDetails
                    {
                        ExitIdHeader = Convert.ToInt32(Result["ExitIdHeader"]),
                        ExitIdDtls = Convert.ToInt32(Result["ExitIdDtls"].ToString()),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstExitThroughGateDtls;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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


        public void GetExitThroughGateDetailsForHdr(int HeaderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExitIdHrd", MySqlDbType = MySqlDbType.Int32, Value = HeaderId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetExitThroughGateDetailsForHdr", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_ExitThroughGateDetails> LstExitThroughGateDtls = new List<Dnd_ExitThroughGateDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstExitThroughGateDtls.Add(new Dnd_ExitThroughGateDetails
                    {
                        ExitIdHeader = Convert.ToInt32(Result["ExitIdHeader"]),
                        ExitIdDtls = Convert.ToInt32(Result["ExitIdDtls"].ToString()),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstExitThroughGateDtls;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void GetExitThroughGate(int ExitIdHdr)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExitIdHdr", MySqlDbType = MySqlDbType.Int32, Value = ExitIdHdr });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditExitThroughGate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Dnd_ExitThroughGateHeader ObjExitThroughGateHeader = new Dnd_ExitThroughGateHeader();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjExitThroughGateHeader.ExitIdHeader = Convert.ToInt32(Result["ExitIdHeader"]);
                    ObjExitThroughGateHeader.GateExitNo = Result["GateExitNo"].ToString();
                    ObjExitThroughGateHeader.GatePassNo = Result["GatePassNo"].ToString();//
                    ObjExitThroughGateHeader.GatePassId = Convert.ToInt32((Result["GatePassId"] == DBNull.Value ? 0 : Result["GatePassId"]).ToString());
                    //Result["GatePassId"].ToString()

                    ObjExitThroughGateHeader.GateExitDateTime = Result["GateExitDateTime"].ToString();
                    //ObjEntryThroughGate.EntryDateTime = Result["EntryDateTime"].ToString();
                    //ObjEntryThroughGate.EntryDateTime=Convert.ToString(Result["EntryDateTime"] == null ? "" : Result["EntryDateTime"]);
                    ObjExitThroughGateHeader.GatePassDate = Result["GatePassDate"].ToString();
                    ObjExitThroughGateHeader.ViaId = Convert.ToInt32(Result["ViaId"]);
                    ObjExitThroughGateHeader.Via = Convert.ToString(Result["Via"]);
                    ObjExitThroughGateHeader.Vessel = Convert.ToString(Result["Vessel"]);
                    ObjExitThroughGateHeader.PortId = Convert.ToInt32(Result["PortId"]);
                    ObjExitThroughGateHeader.PortName = Convert.ToString(Result["PortName"]);




                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjExitThroughGateHeader;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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


        public void EditExitThroughGateDetails(int ExitIdDtls)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExitIdDtls", MySqlDbType = MySqlDbType.Int32, Value = ExitIdDtls });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditExitThroughGateDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Dnd_ExitThroughGateDetails ObjExitThroughGateDetails = new Dnd_ExitThroughGateDetails();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjExitThroughGateDetails.ExitIdDtls = Convert.ToInt32(Result["ExitIdDtls"]);
                    ObjExitThroughGateDetails.ExitIdHeader = Convert.ToInt32(Result["ExitIdHeader"]);
                    ObjExitThroughGateDetails.ContainerNo = Result["ContainerNo"].ToString();

                    ObjExitThroughGateDetails.Size = Result["Size"].ToString();
                    ObjExitThroughGateDetails.Reefer = Convert.ToBoolean(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]);
                    ObjExitThroughGateDetails.ShippingLine = Result["ShippingLine"].ToString();
                    //if (Result["shippingLineID"] != null && Result["shippingLineID"] != "")
                    //{
                    ObjExitThroughGateDetails.ShippingLineId = Convert.ToInt32((Result["shippingLineID"] == DBNull.Value ? 0 : Result["shippingLineID"]).ToString());
                    // }
                    //Convert.ToInt32(Result["shippingLineID"].ToString());
                    ObjExitThroughGateDetails.CFSCode = Result["CFSCode"].ToString();
                    ObjExitThroughGateDetails.CHAName = Result["CHAName"].ToString();
                    ObjExitThroughGateDetails.CargoDescription = Result["CargoDescription"].ToString();
                    ObjExitThroughGateDetails.CargoType = Convert.ToInt32(Result["CargoType"].ToString());
                    ObjExitThroughGateDetails.VehicleNo = Result["VehicleNo"].ToString();
                    ObjExitThroughGateDetails.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"].ToString());
                    ObjExitThroughGateDetails.GrossWeight = Convert.ToDecimal(Result["GrossWeight"].ToString());
                    ObjExitThroughGateDetails.DepositorName = Result["DepositorName"].ToString();
                    ObjExitThroughGateDetails.Remarks = Result["Remarks"].ToString();
                    ObjExitThroughGateDetails.Driver = Result["DriverName"].ToString();
                    ObjExitThroughGateDetails.Contact = Result["ContactNo"].ToString();
                    ObjExitThroughGateDetails.PlugInDatetime = Result["PlugInDatetime"].ToString();
                    ObjExitThroughGateDetails.PlugOutDatetime = Result["PlugOutDatetime"].ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjExitThroughGateDetails;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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


        public void GetDetailsForGateExitMail(string LineId)
        {
            int Status = 0;
            int ShippingLineId = int.Parse(LineId);
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Size = 45, Value = ShippingLineId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDetailsForGateExitMail", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            EntryThroughGateMail objMail = new EntryThroughGateMail();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objMail.Email = Convert.ToString(Result["Email"]);
                    objMail.FileName = Convert.ToString(Result["FileName"]);
                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                            objMail.lstExcelData.Add(new ExcelData()
                            {
                                Line = Convert.ToString(Result["Line"]),
                                ContainerNumber = Convert.ToString(Result["ContainerNo"]),
                                Size = Convert.ToString(Result["Size"]),
                                MoveCode = Convert.ToString(Result["MoveCode"]),
                                EntryDateTime = Convert.ToString(Result["EntryDateTime"]),
                                CurrentLocation = Convert.ToString(Result["CurrentLocation"]),
                                ToLocation = Convert.ToString(Result["ToLocation"]),
                                BookingRefNo = Convert.ToString(Result["BookingReferenceNo"]),
                                Customer = Convert.ToString(Result["Customer"]),
                                Transporter = Convert.ToString(Result["Transporter"]),
                                TruckNumber = Convert.ToString(Result["TruckNumber"]),
                                Condition = Convert.ToString(Result["Conditn"]),
                                ReportedBy = Convert.ToString(Result["ReportedBy"]),
                                ReportDate = Convert.ToString(Result["ReportDate"]),
                                Remarks = Convert.ToString(Result["Remarks"]),
                                TransportMode = Convert.ToString(Result["TransportMode"]),
                                JobOrder = Convert.ToString(Result["JobOrderNo"])
                            });
                        }
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objMail;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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

        public void GetDetailsForGateExitMailPIL(string LineId)
        {
            int Status = 0;
            int ShippingLineId = int.Parse(LineId);
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Size = 45, Value = ShippingLineId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDetailsForGateExitMailPIL", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            EntryThroughGateMailPIL objMail = new EntryThroughGateMailPIL();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objMail.Email = Convert.ToString(Result["Email"]);
                    objMail.FileName = Convert.ToString(Result["FileName"]);
                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                            objMail.lstExcelData.Add(new ExcelDataPIL()
                            {
                                Sr = Convert.ToInt32(Result["Sr"]),
                                Line = Convert.ToString(Result["Line"]),

                            });
                        }
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objMail;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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

        public void ContainerForGAtePass(int gatePassId)
        {

            //DateTime dtfrom = DateTime.ParseExact(ObjChequeSummary.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            //DateTime dtTo = DateTime.ParseExact(ObjChequeSummary.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodTo = dtTo.ToString("yyyy/MM/dd");


            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_GateId", MySqlDbType = MySqlDbType.VarChar, Value = gatePassId });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GatePassContainerDetails", CommandType.StoredProcedure, DParam);
            IList<Dnd_ContainerForGp> LstcontainerAgainstGp = new List<Dnd_ContainerForGp>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstcontainerAgainstGp.Add(new Dnd_ContainerForGp
                    {



                        ContainerName = Result["ContainerNo"].ToString(),

                        IsReefer = Convert.ToInt32(Result["IsReefer"].ToString()),

                        size = Convert.ToInt32(Result["size"].ToString() == "" ? "0" : Result["size"].ToString() == null ? "0" : Result["size"].ToString()),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                        NoOfUnits = Result["NoOfUnits"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        Weight = Result["Weight"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        shippingLine = Result["shippingLine"].ToString(),
                        ShippingLineId = Result["ShippingLineId"].ToString(),
                        OperationType = Result["OperationType"].ToString(),
                        GatePassDate = Result["GatePassDate"].ToString(),
                        CHAName = (Result["CHAName"] == null ? "" : Result["CHAName"]).ToString(),
                        DriverName = (Result["DriverName"] == null ? "" : Result["DriverName"]).ToString(),
                        ContactNo = (Result["ContactNo"] == null ? "" : Result["ContactNo"]).ToString(),
                        IsBond = Convert.ToBoolean(Result["IsBond"]),
                        ViaId=Convert.ToInt32(Result["ViaId"]),
                        Via=Convert.ToString(Result["Via"]),
                        Vessel= Convert.ToString(Result["Vessel"]),
                        PortId=Convert.ToInt32(Result["PortId"]),
                        PortName=Convert.ToString(Result["PortName"])
                    });
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstcontainerAgainstGp;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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



        public void GetGatePassLstToRevalidate()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfGatepassToExtend", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<RevalidateGatePass> LstRevalidateGatePass = new List<RevalidateGatePass>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstRevalidateGatePass.Add(new RevalidateGatePass
                    {
                        //  DateTime dt = DateTime.ParseExact(sDateTime, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        GatePassId = Convert.ToInt32(Result["GatePassId"].ToString()),
                        GatePassNo = Result["GatePassNo"].ToString(),
                        ExpiryDT = Result["ExpiryDT"].ToString(),
                        //Convert.ToDateTime(Result["ExpiryDT"].ToString()),
                        DeliveryDate = Result["DeliveryDate"].ToString()
                        //DateTime.ParseExact(Result["DeliveryDate"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        //Convert.ToDateTime(Result["DeliveryDate"].ToString())

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstRevalidateGatePass;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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



        public void UpdateGatePassValidity(RevalidateGatePass objRevalidateGatePass)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = objRevalidateGatePass.GatePassId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpiryDT", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objRevalidateGatePass.ExpiryDT).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("UpdateGatePassDate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Gate Pass Expiry Date Updated Successfully";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Gate Pass  Expiry Date Can Not Be Updated ";
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


        public void ContainerForGAtePass_Ppg(int gatePassId)
        {

            //DateTime dtfrom = DateTime.ParseExact(ObjChequeSummary.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            //DateTime dtTo = DateTime.ParseExact(ObjChequeSummary.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodTo = dtTo.ToString("yyyy/MM/dd");


            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_GateId", MySqlDbType = MySqlDbType.VarChar, Value = gatePassId });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GatePassContainerDetails", CommandType.StoredProcedure, DParam);
            IList<Dnd_ContainerForGp> LstcontainerAgainstGp = new List<Dnd_ContainerForGp>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstcontainerAgainstGp.Add(new Dnd_ContainerForGp
                    {



                        ContainerName = Result["ContainerNo"].ToString(),

                        IsReefer = Convert.ToInt32(Result["IsReefer"].ToString()),

                        size = Convert.ToInt32(Result["size"].ToString()),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                        NoOfUnits = Result["NoOfUnits"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        Weight = Result["Weight"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        shippingLine = Result["shippingLine"].ToString(),
                        ShippingLineId = Result["ShippingLineId"].ToString(),
                        OperationType = Result["OperationType"].ToString(),

                        CHAName = Result["CHAName"].ToString(),
                        GPDate = Result["GPDate"].ToString(),


                    });
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstcontainerAgainstGp;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region CBT GateExit

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
                        GateInDate=Convert.ToString(Result["GateInDate"]),
                        GateInTime=Convert.ToString(Result["GateInTime"])


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


        public void AddEditExitThroughGateCBT(DndGateExitCBT ObjExitThroughGateHeader, int Uid)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_CBTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjExitThroughGateHeader.CBTNo==null?null:ObjExitThroughGateHeader.CBTNo.ToUpper() });
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
            List<DndGateExitCBT> LstExitThroughGate = new List<DndGateExitCBT>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstExitThroughGate.Add(new DndGateExitCBT
                    {
                        GateExitNo = Result["GateExitNo"].ToString(),
                        GateExitDateTime = Result["GateExitDateTime"].ToString(),
                        CBTNo = Result["CBTNo"].ToString(),
                        CFSCode=Result["CFSCode"].ToString(),
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
            DndGateExitCBT ObjExitThroughGate = new DndGateExitCBT();
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
            List<DndGateExitCBT> LstExitThroughGate = new List<DndGateExitCBT>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstExitThroughGate.Add(new DndGateExitCBT
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