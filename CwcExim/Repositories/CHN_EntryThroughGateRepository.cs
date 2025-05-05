using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using System.Data;
using CwcExim.Areas.ExpSealCheking.Models;
using System.Globalization;
using System.IO;
using CwcExim.Models;
using CwcExim.Areas.GateOperation.Models;
using System.Web.Mvc;

namespace CwcExim.Repositories
{
    public class CHN_EntryThroughGateRepository
    {

        private DatabaseResponse _DBResponse;

        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        #region GateEntry

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
            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objEntryThroughGate.EntryDateTime = Result["CurrentDate"].ToString();
                    objEntryThroughGate.Time = Result["CurrentTime"].ToString();
                    objEntryThroughGate.SystemDateTime = Result["CurrentDate"].ToString();


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void ExpSealGetTime()
        {
            int Status = 0;
           
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDateTime", CommandType.StoredProcedure, null);
            _DBResponse = new DatabaseResponse();
            CHN_EntryThroughGate objEntryThroughGate = new CHN_EntryThroughGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objEntryThroughGate.EntryDateTime = Result["CurrentDate"].ToString();
                    objEntryThroughGate.Time = Result["CurrentTime"].ToString();
                    objEntryThroughGate.SystemDateTime = Result["CurrentDate"].ToString();


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
        public void ListOfCHA()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "CHA" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfCHAForSealChecking", CommandType.StoredProcedure, DParam);
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
                        CHAId = Convert.ToInt32(result["CHAId"]),
                        CHAName = result["CHAName"].ToString()
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

        public void ListOfExporter()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "Exporter" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfExporterForSealChecking", CommandType.StoredProcedure, DParam);
            IList<Exporter> lstExporter = new List<Exporter>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstExporter.Add(new Exporter
                    {
                        ExporterId = Convert.ToInt32(result["ExporterId"]),
                        ExporterName = result["ExporterName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstExporter;
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

        public void AddEditEntryThroughGate(CHN_EntryThroughGate ObjEntryThroughGate)
        {
            //if(ObjEntryThroughGate.EntryDateTime != null && ObjEntryThroughGate.EntryDateTime != "")
            //{ }
            //if (ObjEntryThroughGate.ReferenceDate != null && ObjEntryThroughGate.ReferenceDate != "")
            //{ }
            //DateTime EntryDateTm = DateTime.ParseExact(ObjEntryThroughGate.EntryDateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime EntryDateTm =DateTime. ObjEntryThroughGate.EntryDateTime;
            DateTime? TruckSlipDate = null;
            DateTime? ReferenceDt = null;
            var Entrydt = (dynamic)null;
            var SysDateTime = (dynamic)null;
            if (ObjEntryThroughGate.EntryDateTime != null && ObjEntryThroughGate.EntryDateTime != "")
            {
                Entrydt = DateTime.ParseExact(ObjEntryThroughGate.EntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            }
            if (ObjEntryThroughGate.SystemDateTime != null && ObjEntryThroughGate.SystemDateTime != "")
            {
                SysDateTime = DateTime.ParseExact(ObjEntryThroughGate.SystemDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            }
            if (ObjEntryThroughGate.TruckSlipDate != null && ObjEntryThroughGate.TruckSlipDate != "")
            {
                TruckSlipDate = DateTime.ParseExact(ObjEntryThroughGate.TruckSlipDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo == null ? "" : ObjEntryThroughGate.ContainerNo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateInNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.GateInNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Entrydt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ReferenceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceDate", MySqlDbType = MySqlDbType.DateTime, Value = ReferenceDt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SystemDateTime", MySqlDbType = MySqlDbType.DateTime, Value = SysDateTime });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CBT", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjEntryThroughGate.CBT) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsODC", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjEntryThroughGate.IsODC) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLine", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ShippingLine });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo1", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ContainerNo1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = ObjEntryThroughGate.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Reefer", MySqlDbType = MySqlDbType.Bit, Value = ObjEntryThroughGate.Reefer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ShippingLineSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.VehicleNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChallanNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ChallanNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjEntryThroughGate.CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjEntryThroughGate.NoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.GrossWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositorName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.DepositorName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = "Loaded" });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = "Export" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = ObjEntryThroughGate.LCLFCL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.ExporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Exporter", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.Exporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DrivingLicenseNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.DrivingLicenseNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipDate", MySqlDbType = MySqlDbType.DateTime, Value = TruckSlipDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.TruckSlipNo });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditGateEntrySealCheking", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Entry Through Gate Saved Successfully" : "Entry Through Gate Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Duplicate Container No.";
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Data Can't Be Edited as It's Custom Checking Has Been Done.";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Entry Through Gate Could Not Be Saved";
                }
            }
            catch (Exception ex)
            {
                var file = (dynamic)null;
                string Error = ex.InnerException.Message.ToString();
                string CuurDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("/", "");
                string time = DateTime.Now.ToString("H:mm").Replace(":", "");

                string FolderPath2 = HttpContext.Current.Server.MapPath("~/GateError/EntryError/" + CuurDate);
                if (!System.IO.Directory.Exists(FolderPath2))
                {
                    System.IO.Directory.CreateDirectory(FolderPath2);

                }
                file = Path.Combine(FolderPath2, time + "_ErrorEntry.txt");


                using (var tw = new StreamWriter(file, true))
                {
                    tw.WriteLine(Error);
                    tw.Close();
                }

                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void GetAllEntryThroughGate()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGateSealChecking", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_EntryThroughGate> LstEntryThroughGate = new List<CHN_EntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new CHN_EntryThroughGate
                    {

                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        // PrintSealCut = Result["PrintSealCut"].ToString(),
                        // CBTContainer = Result["CBTContainer"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetEntryThroughGate(int EntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditEntryThroughGateSealChecking", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_EntryThroughGate ObjEntryThroughGate = new CHN_EntryThroughGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjEntryThroughGate.EntryId = Convert.ToInt32(Result["EntryId"]);
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    //ObjEntryThroughGate.CFSCode = Result["CFSCode"].ToString();
                    ObjEntryThroughGate.GateInNo = Result["GateInNo"].ToString();
                    //ObjEntryThroughGate.EntryDateTime = Result["EntryDateTime"].ToString();
                    ObjEntryThroughGate.EntryDateTime = Convert.ToString(Result["EntryDateTime"] == null ? "" : Result["EntryDateTime"]);
                    //ObjEntryThroughGate.ReferenceDate= Result["ReferenceDate"].ToString();
                    ObjEntryThroughGate.TruckSlipDate = Convert.ToString(Result["TruckSlipDate"] == null ? "" : Result["TruckSlipDate"]);
                    ObjEntryThroughGate.SystemDateTime = Convert.ToString(Result["SystemDateTime"]);
                    // ObjEntryThroughGate.CBT = Convert.ToBoolean(Result["CBT"] == DBNull.Value ? 0 : Result["CBT"]);
                    // ObjEntryThroughGate.IsODC = Convert.ToBoolean(Result["IsODC"] == DBNull.Value ? 0 : Result["IsODC"]);
                    // ObjEntryThroughGate.ShippingLine = Result["ShippingLine"].ToString();
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.CHAId = Convert.ToInt32(Result["CHAId"]);
                    //ObjEntryThroughGate.ContainerNo1 = (Result["ContainerNo1"] == null ? "" : Result["ContainerNo1"]).ToString();
                    // ObjEntryThroughGate.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    // ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);

                    ObjEntryThroughGate.Size = Result["Size"].ToString();
                    //ObjEntryThroughGate.Reefer =Convert.ToBoolean(Result["Reefer"].ToString());

                    // ObjEntryThroughGate.Reefer = Convert.ToBoolean(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]);

                    ObjEntryThroughGate.CustomSealNo = Result["CustomSealNo"].ToString();
                    ObjEntryThroughGate.TruckSlipNo = Result["TruckSlipNo"].ToString();
                    ObjEntryThroughGate.VehicleNo = Result["VehicleNo"].ToString();
                    ObjEntryThroughGate.DrivingLicenseNo = Result["DrivingLicenseNo"].ToString();

                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    ObjEntryThroughGate.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWeight"].ToString());
                    ObjEntryThroughGate.Exporter = Result["Exporter"].ToString();
                    ObjEntryThroughGate.ExporterId = Convert.ToInt32(Result["ExporterId"]);
                    ObjEntryThroughGate.Remarks = Result["Remarks"].ToString();
                    ObjEntryThroughGate.CargoDescription = Result["CargoDescription"].ToString();
                    //ObjEntryThroughGate.ContainerType = Result["ContainerType"].ToString();
                    //ObjEntryThroughGate.OperationType = Result["OperationType"].ToString();


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void DeleteEntryThroughGate(int EntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("DeleteGateEntrySealChecking", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Gate Entry Deleted Successfully";
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete As It Is Used In Another Form";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete As It's Custom Checking has Been Done";
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

        public void GateEntryTruckSlipReport(int EntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintGateEntryTruckSlip", CommandType.StoredProcedure, DParam);
            PrintTruckSlip objTP = new PrintTruckSlip();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objTP.GateInNo = Convert.ToString(Result["GateInNo"]);
                    objTP.TruckSlipNo = Result["TruckSlipNo"].ToString();
                    objTP.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    objTP.ChaName = (Result["CHAName"] == null ? "" : Result["CHAName"]).ToString();
                    objTP.Exporter = (Result["Exporter"] == null ? "" : Result["Exporter"]).ToString();
                    objTP.ContainerNo = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString();
                    objTP.Size = (Result["Size"] == null ? "" : Result["Size"]).ToString();
                    objTP.Entrydate = (Result["Entrydate"] == null ? "" : Result["Entrydate"]).ToString();
                    objTP.Cargo = Result["CargoDescription"].ToString();
                    objTP.Remarks = Result["Remarks"].ToString();
                    objTP.NoOfUnits = Result["NoOfPackages"].ToString();
                    objTP.CustomSealNo = Result["CustomSealNo"].ToString();
                    objTP.EntryTime = Result["EntryTime"].ToString();
                    objTP.VehicleNo = Result["VehicleNo"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objTP;
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

        public void GetDetailsForGatePassPrint(int GateEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = GateEntryId });
          //  lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEntryPrint", CommandType.StoredProcedure, DParam);
            CHN_GateEntryPrint objGP = new CHN_GateEntryPrint();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objGP.CompanyAdrress = Result["CompAdress"].ToString();
                    objGP.CompanyMail = Result["CompEmail"].ToString();
                    objGP.CompanyShortName = Result["CompShortNm"].ToString();
                    objGP.CompanyLocation = Result["CompLocation"].ToString();
                    objGP.GateInNo = Convert.ToString(Result["GateInNo"]);
                    objGP.EntryDate = Convert.ToString(Result["EntryDate"]);
                    objGP.EntryTime = Convert.ToString(Result["EntryTime"]);
                    objGP.ShippingLine = Convert.ToString(Result["ShippingLine"]);
                    objGP.CHAName = Convert.ToString(Result["CHAName"]);
                    objGP.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    objGP.CustomSealNo = Convert.ToString(Result["CustomSealNo"]);
                    objGP.ShippingLineSealNo = Convert.ToString(Result["ShippingLineSealNo"]);
                    objGP.VehicleNo = Convert.ToString(Result["VehicleNo"]);
                    objGP.CargoDescription = Convert.ToString(Result["CargoDescription"]);
                    objGP.CargoType = Convert.ToString(Result["CargoType"]);
                    objGP.NoOfPackages = Convert.ToString(Result["NoOfPackages"]);
                    objGP.GrossWeight = Convert.ToString(Result["GrossWeight"]);
                    objGP.ContainerLoadType = Convert.ToString(Result["ContainerLoadType"]);
                    objGP.ContainerType = Convert.ToString(Result["ContainerType"]);
                    objGP.OperationType = Convert.ToString(Result["OperationType"]);
                    objGP.DisplayCfs = Convert.ToString(Result["DisplayCfs"]);
                    objGP.Remarks = Convert.ToString(Result["Remarks"]);
                    objGP.Size = Convert.ToString(Result["Size"]);
                    objGP.TerminalDate = Convert.ToString(Result["TerminalOutDate"]);
                    objGP.TerminalTime = Convert.ToString(Result["TerminalOutTime"]);


                }
                
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objGP;
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

        #region Custom Checking Approval
        public void GetContainerDetForCustomChecking(string TruckSlipNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = TruckSlipNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerDetForCustomChecking", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_CustomChekingApproval ObjEntryThroughGate = new CHN_CustomChekingApproval();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.Size = Result["Size"].ToString();
                    ObjEntryThroughGate.TruckSlipNo = Result["TruckSlipNo"].ToString();
                    ObjEntryThroughGate.VehicleNo = Result["VehicleNo"].ToString();
                    ObjEntryThroughGate.TruckSlipDate = Result["TruckSlipDate"].ToString();
                    ObjEntryThroughGate.GateEntryId = Convert.ToInt32(Result["EntryId"].ToString());
                    ObjEntryThroughGate.CFSCode = Result["CFSCode"].ToString();
                    ObjEntryThroughGate.BranchId = Convert.ToInt32(Result["BranchId"].ToString());
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void ListOfTrcukSlipForCustomChecking()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "TruckSlipNo" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetTruckSlipForCustomChecking", CommandType.StoredProcedure, DParam);
            IList<TruckSlip> lstTruckSlip = new List<TruckSlip>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstTruckSlip.Add(new TruckSlip
                    {
                        TruckSlipNo = result["TruckSlipNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstTruckSlip;
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

        public void AddEditCustomChekingApproval(CHN_CustomChekingApproval ObjEntryThroughGate)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.CustomId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateEntryId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.GateEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChaName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = ObjEntryThroughGate.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.VehicleNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExamRequired", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ExamRequired });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.TruckSlipNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditCustomChekingApproval", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Custom Checking Approval Saved Successfully" : "Custom Checking Approval Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Truck Slip No Already Exists.";
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Data Can't Be Edited As It's Further Process Has Been Done.";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Custom Checking Approval Could Not Be Saved";
                }
            }
            catch (Exception ex)
            {

                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void GetAllCustomChekingApproval()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_CustomId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //  IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllCustomChekingApproval", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            List<CHN_CustomChekingApproval> LstEntryThroughGate = new List<CHN_CustomChekingApproval>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new CHN_CustomChekingApproval
                    {
                        CustomId = Convert.ToInt32(Result["CustomId"].ToString()),
                        TruckSlipNo = Result["TruckSlipNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ExamRequired = Result["ExamRequired"].ToString(),
                        // CBTContainer = Result["CBTContainer"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetCustomChekingApproval(int CustomId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomId", MySqlDbType = MySqlDbType.Int32, Value = CustomId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditCustomChekingApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_CustomChekingApproval ObjEntryThroughGate = new CHN_CustomChekingApproval();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjEntryThroughGate.CustomId = Convert.ToInt32(Result["CustomId"]);
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.VehicleNo = Result["VehicleNo"].ToString();
                    ObjEntryThroughGate.Size = Result["Size"].ToString();
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.TruckSlipNo = Result["TruckSlipNo"].ToString();
                    ObjEntryThroughGate.ExamRequired = Result["ExamRequired"].ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void DeleteCustomCheckingApproval(int CustomId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CustomId", MySqlDbType = MySqlDbType.Int32, Value = CustomId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("DeleteCustomCheckingApproval", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Custom Checking Approval Entry Deleted Successfully";
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete As It Is Used In Another Form";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete As It's Job Order Has Been Done";
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

        #region Job Order
        public void ListOfTrcukSlipForJobOrder()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "TruckSlipNo" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GettruckSlipForJobOrder", CommandType.StoredProcedure, DParam);
            IList<TruckSlip> lstTruckSlip = new List<TruckSlip>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstTruckSlip.Add(new TruckSlip
                    {
                        TruckSlipNo = result["TruckSlipNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstTruckSlip;
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


        public void GetContainerDetForJobOrder(string TruckSlipNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = TruckSlipNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerDetForJobOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_SealChekingJobOrder ObjSealCheckingJO = new CHN_SealChekingJobOrder();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSealCheckingJO.TruckSlipNo = Result["TruckSlipNo"].ToString();
                    ObjSealCheckingJO.TruckSlipDate = Result["TruckSlipDate"].ToString();
                    ObjSealCheckingJO.ContainerNo = Result["ContainerNo"].ToString();
                    ObjSealCheckingJO.Size = Result["Size"].ToString();
                    ObjSealCheckingJO.CustomId = Convert.ToInt32(Result["CustomId"].ToString());
                    ObjSealCheckingJO.BranchId = Convert.ToInt32(Result["BranchId"].ToString());
                    ObjSealCheckingJO.CFSCode = Result["CFSCode"].ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSealCheckingJO;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void AddEditSealCheckingJO(CHN_SealChekingJobOrder objJO, string XML = null, string LocationXML = null, string ClauseXML = null)
        {
            //string GeneratedClientId = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objJO.ImpJobOrderId) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipNo", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objJO.TruckSlipNo });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_FromContainer", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objJO.FromContainer) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderNo", MySqlDbType = MySqlDbType.VarChar, Size = 2, Value = Convert.ToString(objJO.JobOrderNo) });
            //   lstParam.Add(new MySqlParameter { ParameterName = "in_OperationId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objJO.OperationId) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objJO.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objJO.TruckSlipDate).ToString("yyyy-MM-dd") });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DtlXml", MySqlDbType = MySqlDbType.Text, Value = XML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CXml", MySqlDbType = MySqlDbType.Text, Value = ClauseXML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CustomId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objJO.CustomId) });
            //  lstParam.Add(new MySqlParameter { ParameterName = "in_LctnXML", MySqlDbType = MySqlDbType.Text, Value = LocationXML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            // LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditSealCheckingJO", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Export Seal Checking Job Order Saved Successfully" : "Export Seal Checking Job Order Updated Successfully";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = "Truck Slip No Already Exists.";
                }
                else if (result == 4 || result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = "Data Can't Be Edited As It's Further Process Has Been Done.";
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

        public void GetAllJobOrder()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_CustomId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //  IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfSealCheckingJobOrder", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            List<CHN_JobOrderList> LstEntryThroughGate = new List<CHN_JobOrderList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new CHN_JobOrderList
                    {
                        JobOrderId = Convert.ToInt32(Result["JobOrderId"].ToString()),
                        TruckSlipNo = Result["TruckSlipNo"].ToString(),
                        //Size = Result["Size"].ToString(),
                        TruckSlipDate = Result["TruckSlipDate"].ToString(),
                        JobOrderNo = Result["JobOrderNo"].ToString(),
                        JobOrderDate = Result["JobOrderDate"].ToString(),
                        // ExamRequired = Result["ExamRequired"].ToString(),
                        // CBTContainer = Result["CBTContainer"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetJobOrderById(int ImpJobOrderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ImpJobOrderId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditSealCheckingJobOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_SealChekingJobOrder ObjSealChecking = new CHN_SealChekingJobOrder();
            IList<CHN_JobOrderClauseDtl> lstCDtl = new List<CHN_JobOrderClauseDtl>();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSealChecking.ImpJobOrderId = Convert.ToInt32(Result["JobOrderId"]);
                    ObjSealChecking.ContainerNo = Result["ContainerNo"].ToString();
                    ObjSealChecking.JobOrderDate = Result["JobOrderDate"].ToString();
                    ObjSealChecking.TruckSlipDate = Convert.ToString(Result["TruckSlipDate"] == null ? "" : Result["TruckSlipDate"]);
                    ObjSealChecking.JobOrderNo = Result["JobOrderNo"].ToString();
                    ObjSealChecking.Size = Result["ContainerSize"].ToString();
                    ObjSealChecking.TruckSlipNo = Result["TruckSlipNo"].ToString();



                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstCDtl.Add(new CHN_JobOrderClauseDtl
                        {
                            OperationId = Convert.ToInt32(Result["Clause"]),
                            OperationCode = Result["OperationCode"].ToString(),
                        });
                    }
                }

                if (lstCDtl.Count > 0)
                {
                    ObjSealChecking.StringifyClauseXML = Newtonsoft.Json.JsonConvert.SerializeObject(lstCDtl);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSealChecking;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void DeleteSealCheckingJO(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("DeleteSealCheckingJO", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "SealChecking Job Order Deleted Successfully";
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete As It Is Used In Another Form";
                }
                else if (result == 3 || result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete As It's Further Process has Been Done";
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

        public void PrintSealChekingJobOrder(int JobOrderId, string TruckSlipNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_JoborderId", MySqlDbType = MySqlDbType.Int32, Value = JobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipNo", MySqlDbType = MySqlDbType.VarChar,Size=30, Value = TruckSlipNo });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintSealChekingJobOrder", CommandType.StoredProcedure, DParam);
            PrintJobOrderSealChecking objTP = new PrintJobOrderSealChecking();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objTP.JobOrderNo = Result["JobOrderNo"].ToString();
                    objTP.Exporter = (Result["Exporter"] == null ? "" : Result["Exporter"]).ToString();
                    objTP.weight = Convert.ToDecimal(Result["GrossWeight"].ToString());
                    objTP.JobOrderDate = (Result["JobOrderDate"] == null ? "" : Result["JobOrderDate"]).ToString();
                    objTP.NoOfUnits =Convert.ToInt32(Result["NoOfPackages"].ToString());
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objTP;
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

        #region Seal Change Entry

        public void ListOfJobOrderNo()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "TruckSlipNo" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetSealCheckingJobOrderNo", CommandType.StoredProcedure, DParam);
            IList<JobOrderList> lstJobOrder = new List<JobOrderList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstJobOrder.Add(new JobOrderList
                    {
                        JobOrderNo = result["JobOrderNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstJobOrder;
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

        public void GetContainerDetByJobOrderNo(string JobOrderNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = JobOrderNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerDetByJobOrderNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_SealChangeEntry ObjSealChange = new CHN_SealChangeEntry();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSealChange.JobOrderid = Convert.ToInt32(Result["JobOrderId"].ToString());
                    ObjSealChange.JobOrderDate = Result["JobOrderDate"].ToString();
                    ObjSealChange.ContainerNo = Result["ContainerNo"].ToString();
                    ObjSealChange.Size = Result["ContainerSize"].ToString();
                    ObjSealChange.TruckSlipNo = Result["TruckSlipNo"].ToString();
                    ObjSealChange.TruckSlipDate = Result["TruckSlipDate"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSealChange;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void AddEditSealChangeEntry(CHN_SealChangeEntry ObjEntrySealChange)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjEntrySealChange.EntryId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntrySealChange.ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = ObjEntrySealChange.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderNo", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntrySealChange.JobOrderNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjEntrySealChange.JobOrderDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntrySealChange.TruckSlipNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjEntrySealChange.TruckSlipDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PresentSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntrySealChange.PresentSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntrySealChange.NewSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LockProvided", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Convert.ToInt32(ObjEntrySealChange.LockProvided) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjEntrySealChange.JobOrderid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjEntrySealChange.Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditSealChangeEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Seal Change Saved Successfully" : "Seal Change Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Truck Slip No Already Exists.";
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Seal Change Can't Be Edited As It's Further Process Has Been Done .";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Seal Change Could Not Be Saved";
                }
            }
            catch (Exception ex)
            {

                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void GetAllSealChangeEntry()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_CustomId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //  IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllSealChangeEntry", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            List<CHN_SealChangeEntry> LstSealChangeEntry = new List<CHN_SealChangeEntry>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSealChangeEntry.Add(new CHN_SealChangeEntry
                    {
                        EntryId = Convert.ToInt32(Result["EntryId"].ToString()),
                        JobOrderNo = Result["JobOrderNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["ContainerSize"].ToString(),
                        TruckSlipNo = Result["TruckSlipNo"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSealChangeEntry;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetSealChangeEntry(int EntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditSealChangeEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_SealChangeEntry ObjSealChangeEntry = new CHN_SealChangeEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSealChangeEntry.EntryId = Convert.ToInt32(Result["EntryId"]);
                    ObjSealChangeEntry.JobOrderNo = Result["JobOrderNo"].ToString();
                    ObjSealChangeEntry.JobOrderDate = Convert.ToString(Result["JobOrderDate"] == null ? "" : Result["JobOrderDate"]);
                    ObjSealChangeEntry.ContainerNo = Result["ContainerNo"].ToString();
                    ObjSealChangeEntry.Size = Result["ContainerSize"].ToString();
                    ObjSealChangeEntry.TruckSlipNo = Result["TruckSlipNo"].ToString();
                    ObjSealChangeEntry.TruckSlipDate = Convert.ToString(Result["TruckSlipDate"] == null ? "" : Result["TruckSlipDate"]);
                    ObjSealChangeEntry.PresentSealNo = Result["PresentSealNo"].ToString();
                    ObjSealChangeEntry.NewSealNo = Result["NewSealNo"].ToString();
                    ObjSealChangeEntry.LockProvided = Convert.ToBoolean(Result["LockProvided"] == DBNull.Value ? 0 : Result["LockProvided"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSealChangeEntry;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void DeleteSealChangEntry(int EntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("DeleteSealChangeEntry", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Seal Change Entry Deleted Successfully";
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete As It Is Used In Another Form";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete As It's Further Process Has Been Done";
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

        #region Inspection And Weighment
        public void ListOfTrcukSlipForWeighment()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "TruckSlipNo" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GettruckSlipForWeighment", CommandType.StoredProcedure, DParam);
            IList<TruckSlipForWeighment> lstTruckSlip = new List<TruckSlipForWeighment>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstTruckSlip.Add(new TruckSlipForWeighment
                    {
                        TruckSlipNo = result["TruckSlipNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstTruckSlip;
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

        public void GetContainerDetForWeighment(string TruckSlipNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = TruckSlipNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerDetForWeighment", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_Weighment ObjSealInspection = new CHN_Weighment();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // ObjSealInspection.SealChangeEntryId = Convert.ToInt32(Result["EntryId"].ToString());
                    ObjSealInspection.TruckSlipNo = Result["TruckSlipNo"].ToString();
                    ObjSealInspection.TruckSlipDate = Result["TruckSlipDate"].ToString();
                    ObjSealInspection.ContainerNo = Result["ContainerNo"].ToString();
                    ObjSealInspection.Size = Result["ContainerSize"].ToString();
                    ObjSealInspection.BranchId =Convert.ToInt32(Result["BranchId"].ToString());
                    ObjSealInspection.CFSCode = Result["CFSCode"].ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSealInspection;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void AddEditSealCheckingweighent(CHN_Weighment ObjEntryInspection)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_WeighmentId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjEntryInspection.WeighmentId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryInspection.ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = ObjEntryInspection.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryInspection.TruckSlipNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjEntryInspection.TruckSlipDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OnWheelInspection", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryInspection.OnWheelInspection });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjEntryInspection.GrossWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TareWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToInt32(ObjEntryInspection.TareWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjEntryInspection.BranchId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryInspection.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Convert.ToInt32(ObjEntryInspection.Uid) });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_SealChangeEntryId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjEntryInspection.SealChangeEntryId) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditSealCheckingWeighMent", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Seal Checking Weighment Saved Successfully" : "Seal Checking Weighment Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "TruckSlip No Already Exists.";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Seal Checking Weighmene Could  Not Be Saved";
                }
            }
            catch (Exception ex)
            {

                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void GetAllSealCheckingWeighment()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_CustomId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //  IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfSealCheckingWeighment", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            List<CHN_WeighmentList> LstSealChangeEntry = new List<CHN_WeighmentList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSealChangeEntry.Add(new CHN_WeighmentList
                    {
                        WeighmentId = Convert.ToInt32(Result["WeighmentId"].ToString()),
                        TruckSlipNo = Result["TruckSlipNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                        TareWeight = Convert.ToDecimal(Result["TareWeight"].ToString()),




                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSealChangeEntry;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetSealCheckingWeighmentById(int WeighmentId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_WeighmentId", MySqlDbType = MySqlDbType.Int32, Value = WeighmentId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditSealCheckingWeighment", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_Weighment ObjWeighment = new CHN_Weighment();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjWeighment.WeighmentId = Convert.ToInt32(Result["WeighmentId"]);
                    ObjWeighment.TruckSlipNo = Result["TruckSlipNo"].ToString();
                    ObjWeighment.TruckSlipDate = Convert.ToString(Result["TruckSlipDate"] == null ? "" : Result["TruckSlipDate"]);
                    ObjWeighment.ContainerNo = Result["ContainerNo"].ToString();
                    ObjWeighment.Size = Result["ContainerSize"].ToString();
                    ObjWeighment.OnWheelInspection = Result["OnWheelInspection"].ToString();
                    ObjWeighment.GrossWeight = Convert.ToDecimal(Result["GrossWeight"].ToString());
                    ObjWeighment.TareWeight = Convert.ToDecimal(Result["TareWeight"].ToString());

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjWeighment;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void DeleteSealCheckingWeighment(int WeighmentId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_WeighmentId", MySqlDbType = MySqlDbType.Int32, Value = WeighmentId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("DeleteSealCheckingWeighment", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Seal Checking Weighment Deleted Successfully";
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

        #endregion

        #region Shut Out

        public void AddEditShutOut(CHN_ShutOut ObjShutOut)
        {
           
            int RetValue = 0;
            string GeneratedClientId = "0";
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShutOutId", MySqlDbType = MySqlDbType.Int32, Value = ObjShutOut.ShutOutId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TruckslipNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjShutOut.TruckSlipNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjShutOut.TruckSlipDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjShutOut.ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = ObjShutOut.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShutOut", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToInt32(ObjShutOut.ShutOut) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Reason", MySqlDbType = MySqlDbType.VarChar, Value = ObjShutOut.Reason });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjShutOut.UId) });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = RetValue });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            DParam = LstParam.ToArray();
            int result = DataAccess.ExecuteNonQuery("AddEditShutOut", CommandType.StoredProcedure, DParam, out GeneratedClientId);

            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "ShutOut Cargo Saved Successfully" : "Shut Out Cargo Updated Successfully";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = "Truck Slip No Already Exists";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "error";

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

            }
        }


        public void GetTruckSlipNo()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetTruckSlipListForShutOutCargo", CommandType.StoredProcedure, DParam);
            IList<CHN_ShutOut> lstShutOut = new List<CHN_ShutOut>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstShutOut.Add(new CHN_ShutOut
                    {
                        //EntryId = Convert.ToInt32(result["EntryId"].ToString()),
                        TruckSlipNo = result["TruckSlipNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstShutOut;
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

        public void GetShutOutCargoList()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShutOutCargoList", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            List<CHN_ShutOut> LstShutOutList = new List<CHN_ShutOut>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstShutOutList.Add(new CHN_ShutOut
                    {
                        ShutOutId=Convert.ToInt32(Result["ShutOutId"].ToString()),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        ShutOut = Convert.ToBoolean(Result["Shutout"])
                });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstShutOutList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetTruckSlipNoDetails(string TruckSlipNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TruckSlipNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = TruckSlipNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetTruckSlipNoDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_ShutOut ObjShutOut = new CHN_ShutOut();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjShutOut.ContainerNo = Result["ContainerNo"].ToString();
                    ObjShutOut.TruckSlipDate = Result["TruckSlipDate"].ToString();
                    ObjShutOut.Size = Result["Size"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Data = ObjShutOut;
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

        public void GetShutOutCargoById(int ShutOutId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShutOutId", MySqlDbType = MySqlDbType.Int32, Value = ShutOutId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditShutOutCargo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_ShutOut ObjShutOut = new CHN_ShutOut();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjShutOut.ShutOutId = Convert.ToInt32(Result["ShutOutId"]);
                    ObjShutOut.ContainerNo = Result["ContainerNo"].ToString();
                    ObjShutOut.Size = Result["Size"].ToString();
                    ObjShutOut.TruckSlipNo = Result["TruckSlipNo"].ToString();
                    ObjShutOut.TruckSlipDate = Convert.ToString(Result["TruckSlipDate"] == null ? "" : Result["TruckSlipDate"]);
                    ObjShutOut.Reason = Result["Reason"].ToString();
                    ObjShutOut.ShutOut = Convert.ToBoolean(Result["ShutOut"] == DBNull.Value ? 0 : Result["ShutOut"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjShutOut;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void DeleteShutOut(int ShutOutId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShutOutId", MySqlDbType = MySqlDbType.Int32, Value = ShutOutId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("DeleteShutOut", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "ShutOut Cargo Deleted Successfully";
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


        public void AddEditEntryThroughGateCBT(ChnEntryThroughGate ObjEntryThroughGate)
        {
            //if(ObjEntryThroughGate.EntryDateTime != null && ObjEntryThroughGate.EntryDateTime != "")
            //{ }
            //if (ObjEntryThroughGate.ReferenceDate != null && ObjEntryThroughGate.ReferenceDate != "")
            //{ }
            //DateTime EntryDateTm = DateTime.ParseExact(ObjEntryThroughGate.EntryDateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime EntryDateTm =DateTime. ObjEntryThroughGate.EntryDateTime;
            DateTime? ReferenceDt = null;
            var Entrydt = (dynamic)null;
            var SysDateTime = (dynamic)null;
            if (ObjEntryThroughGate.ReferenceDate != null && ObjEntryThroughGate.ReferenceDate != "")
            {
                ReferenceDt = Convert.ToDateTime(ObjEntryThroughGate.ReferenceDate);
            }
            if (ObjEntryThroughGate.EntryDateTime != null && ObjEntryThroughGate.EntryDateTime != "")
            {
                Entrydt = DateTime.ParseExact(ObjEntryThroughGate.EntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            }

            if (ObjEntryThroughGate.SystemDateTime != null && ObjEntryThroughGate.SystemDateTime != "")
            {
                SysDateTime = DateTime.ParseExact(ObjEntryThroughGate.SystemDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            }
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo == null ? "" : ObjEntryThroughGate.ContainerNo.Trim()) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateInNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.GateInNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Entrydt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ReferenceNo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceDate", MySqlDbType = MySqlDbType.DateTime, Value = ReferenceDt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SystemDateTime", MySqlDbType = MySqlDbType.DateTime, Value = SysDateTime });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Size = 30, Value = ObjEntryThroughGate.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLine", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ShippingLine });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 30, Value = ObjEntryThroughGate.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo1", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo1 == null ? "" : ObjEntryThroughGate.ContainerNo1.Trim()) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = ObjEntryThroughGate.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Reefer", MySqlDbType = MySqlDbType.Bit, Value = ObjEntryThroughGate.Reefer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ShippingLineSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.VehicleNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChallanNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ChallanNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjEntryThroughGate.CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.NoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ActualPackages", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.ActualPackages });

            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.GrossWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TareWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.TareWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositorName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.DepositorName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ContainerType });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.OperationType });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportMode", MySqlDbType = MySqlDbType.Int32, Size = 1, Value = ObjEntryThroughGate.TransportMode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerLoadType", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = ObjEntryThroughGate.ContainerLoadType });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportFrom", MySqlDbType = MySqlDbType.VarChar,  Value = ObjEntryThroughGate.TransportFrom });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CBT", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.IsCBT ? 1 : 0 });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_TPNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ObjEntryThroughGate.TPNo });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 100, Direction = ParameterDirection.Output });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditEntryThroughGateCBT", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Entry Through Gate Saved Successfully" : "Entry Through Gate Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Duplicate Container No.";
                }
                else if (Result == -1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Entry Through Gate Could Not Be Saved";
                }
            }
            catch (Exception ex)
            {
                var file = (dynamic)null;
                string Error = ex.InnerException.Message.ToString();
                string CuurDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("/", "");
                string time = DateTime.Now.ToString("H:mm").Replace(":", "");

                string FolderPath2 = HttpContext.Current.Server.MapPath("~/GateError/EntryError/" + CuurDate);
                if (!System.IO.Directory.Exists(FolderPath2))
                {
                    System.IO.Directory.CreateDirectory(FolderPath2);

                }
                file = Path.Combine(FolderPath2, time + "_ErrorEntry.txt");


                using (var tw = new StreamWriter(file, true))
                {
                    tw.WriteLine(Error);
                    tw.Close();
                }

                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        
        public void GetAutoPopulateData(string ContainerNumber)
        {
            ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value =Convert.ToInt32(ContainerNumber) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainerDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            ChnEntryThroughGate ObjEntryThroughGate = new ChnEntryThroughGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                 //   ObjEntryThroughGate.ReferenceNo = Result["ReferenceNo"].ToString();
                   // ObjEntryThroughGate.ReferenceDate = Result["ReferenceDate"].ToString();
                    ObjEntryThroughGate.ShippingLine = Result["ShippingLineName"].ToString();
                   // ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.Size = Result["ContainerSize"].ToString();
                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"]);
                  //  ObjEntryThroughGate.EntryDateTime = Convert.ToDateTime(Result["EntryDateTime"]).ToString("dd/MM/yyyy");
                   // ObjEntryThroughGate.EntryTime = Result["Entime"].ToString();
                    ObjEntryThroughGate.CargoDescription = Result["CargoDesc"].ToString();
                    ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GR_WT"]);
                    ObjEntryThroughGate.NoOfPackages = Convert.ToInt32(Result["NO_PKG"]);
                    ObjEntryThroughGate.ContainerLoadType = Result["MovementType"].ToString();
                    //  ObjEntryThroughGate.Reefer = Convert.ToBoolean(Result["Reefer"].ToString());
                    //  if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 1)
                    //  {
                    //     ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                    // }
                    // else if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 3)
                    // {
                    //  ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                    //ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWt"]);
                    //   ObjEntryThroughGate.ContainerLoadType = Result["LoadType"].ToString();
                    //    ObjEntryThroughGate.ShippingLineSealNo = Result["SLineNo"].ToString();

                    // }
                    ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    //EntryThroughGate objEntryThroughGate = new EntryThroughGate();
                    // objEntryThroughGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
                    //string strDate = ObjEntryThroughGate.ReferenceDate;
                    //string[] arrayDate = strDate.Split(' ');
                    //ObjEntryThroughGate.ReferenceDate = arrayDate[0];
                 //   ObjEntryThroughGate.EntryTime = ObjEntryThroughGate.EntryTime;

                    //ViewBag.strTime = objEntryThroughGate.EntryTime;



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetAutoPopulateDataByTrain(string ContainerNumber)
        {
            ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNumber });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainerDetailsByTrain", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            ChnEntryThroughGate ObjEntryThroughGate = new ChnEntryThroughGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjEntryThroughGate.ReferenceNo = Result["ReferenceNo"].ToString();
                    ObjEntryThroughGate.ReferenceDate = Result["ReferenceDate"].ToString();
                    ObjEntryThroughGate.ShippingLine = Result["ShippingLineName"].ToString();
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.Size = Result["ContainerSize"].ToString();
                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"]);
                    ObjEntryThroughGate.EntryDateTime = Convert.ToDateTime(Result["EntryDateTime"]).ToString("dd/MM/yyyy");
                    ObjEntryThroughGate.EntryTime = Result["Entime"].ToString();
                    ObjEntryThroughGate.CargoDescription = Result["CargoDesc"].ToString();
                    ObjEntryThroughGate.Reefer = Convert.ToBoolean(Convert.ToInt32(Result["Reefer"]));
                    //if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 1)
                    //{
                    //    ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                    //}
                    //else if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 3)
                    //{
                    ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                    ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWt"]);
                    ObjEntryThroughGate.ContainerLoadType = Result["LoadType"].ToString();
                    ObjEntryThroughGate.ShippingLineSealNo = Result["SLineNo"].ToString();
                    ObjEntryThroughGate.TransportFrom = Result["TranFrom"].ToString();
                    ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    ObjEntryThroughGate.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]);
                    ObjEntryThroughGate.Remarks = Convert.ToString(Result["Remarks"]);
                    //}
                    //EntryThroughGate objEntryThroughGate = new EntryThroughGate();
                    // objEntryThroughGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
                    //string strDate = ObjEntryThroughGate.ReferenceDate;
                    //string[] arrayDate = strDate.Split(' ');
                    //ObjEntryThroughGate.ReferenceDate = arrayDate[0];
                    ObjEntryThroughGate.EntryTime = ObjEntryThroughGate.EntryTime;

                    //ViewBag.strTime = objEntryThroughGate.EntryTime;



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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



        public void GetLoadedContData(int LoadContainerRefId)
        {
            //ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadAppId", MySqlDbType = MySqlDbType.Int32, Value = LoadContainerRefId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("DetailsForLoadedContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LoadedContainerListWithData> lstLoadedContainerListWithData = new List<LoadedContainerListWithData>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstLoadedContainerListWithData.Add(new LoadedContainerListWithData
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ShippingLineId = Result["ShippingLineId"].ToString(),
                        ShippingLineName = Result["shippingLine"].ToString(),
                        Size = Result["Size"].ToString(),
                        Reefer = Result["Reefer"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        NoOfUnits = Result["NoOfUnits"].ToString(),
                        GrossWt = Result["GrossWt"].ToString(),
                        CustomSeal= Result["CustomSeal"].ToString()


                    });


                    //ViewBag.strTime = objEntryThroughGate.EntryTime;



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstLoadedContainerListWithData;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
  
      
      

        public void GetAllEntryThroughGateListPage(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = null });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGateListPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ChnEntryThroughGate> LstEntryThroughGate = new List<ChnEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new ChnEntryThroughGate
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
        public void GetAllEntryThroughGateTrain()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = null });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ChnEntryThroughGate> LstEntryThroughGate = new List<ChnEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new ChnEntryThroughGate
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetAllEntryThroughTrainListPage(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = null });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGateListPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ChnEntryThroughGate> LstEntryThroughGate = new List<ChnEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new ChnEntryThroughGate
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
        public void GetAllEntryThroughGateEmpty(string ContainerType)
        {
            int Status = 0;

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = ContainerType = "Empty" });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PpgEntryThroughGate> LstEntryThroughGate = new List<PpgEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new PpgEntryThroughGate
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetAllEntryThroughGateEmptyListPage(int Page, string ContainerType)
        {
            int Status = 0;

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = ContainerType = "Empty" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGateListPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PpgEntryThroughGate> LstEntryThroughGate = new List<PpgEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new PpgEntryThroughGate
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
        public void GetAllEntryThroughGateCBT()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = null });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PpgEntryThroughGate> LstEntryThroughGate = new List<PpgEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new PpgEntryThroughGate
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetAllEntryThroughGateCBTListPage(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = null });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGateListPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ChnEntryThroughGate> LstEntryThroughGate = new List<ChnEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new ChnEntryThroughGate
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetCBT()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllCBT", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<container> Lstcontainer = new List<container>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstcontainer.Add(new container
                    {
                        ContainerName = Result["ContainerNo"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lstcontainer;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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


        public void GetContainer()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_Container> Lstcontainer = new List<CHN_Container>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstcontainer.Add(new CHN_Container
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Id=Convert.ToInt32(Result["Id"])
                         

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lstcontainer;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetContainerByTrain()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainerByTrain", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<container> Lstcontainer = new List<container>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstcontainer.Add(new container
                    {
                        ContainerName = Result["ContainerNo"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lstcontainer;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
        public void GetReferenceNumber()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReferenceNumberExportEmpty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();

            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjReferenceNumber.ReferenceList.Add(new CCINList
                    {
                        CCINId = int.Parse(Convert.ToString(Result["Id"])),
                        CCINNo = Convert.ToString(Result["CCINNo"]),
                        CCINDate = Convert.ToString(Result["CCINDate"]),
                        ShippingLineId = int.Parse(Convert.ToString(Result["ShippingLineId"])),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        NoOfUnits = Convert.ToString(Result["Package"]),
                        Weight = Convert.ToString(Result["Weight"]),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        CHAId = int.Parse(Convert.ToString(Result["CHAId"])),
                        CHAName = Result["CHA"].ToString(),
                    });

                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjReferenceNumber.listExport.Add(new GateEntryExport()
                //        {
                //            ReferenceNo = Convert.ToString(Result["ReferenceNo"]),
                //            ReferenceDate = Convert.ToString(Result["ReferenceDate"]),
                //            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                //            CargoDescription = Convert.ToString(Result["CargoDescription"])

                //        });
                //    }
                //}
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        ObjReferenceNumber.listShippingLine.Add(new ShippingLineList()
                        {
                            ShippingLine = Convert.ToString(Result["ShippingLine"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"])


                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjReferenceNumber;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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


        public void GetBondRefNUmber()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("BondEntryRefNumber", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            //ReferenceNumber ObjReferenceNumber = new ReferenceNumber();
            List<BondReferenceNumberList> lstBondReferenceNumberList = new List<BondReferenceNumberList>();

            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstBondReferenceNumberList.Add(new BondReferenceNumberList
                    {
                        SpaceappId = int.Parse(Convert.ToString(Result["SpaceappId"])),
                        ApplicationNo = Convert.ToString(Result["ApplicationNo"]),
                        ApplicationDate = Convert.ToString(Result["ApplicationDate"]),
                    });

                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjReferenceNumber.listExport.Add(new GateEntryExport()
                //        {
                //            ReferenceNo = Convert.ToString(Result["ReferenceNo"]),
                //            ReferenceDate = Convert.ToString(Result["ReferenceDate"]),
                //            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                //            CargoDescription = Convert.ToString(Result["CargoDescription"])

                //        });
                //    }
                //}
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjReferenceNumber.listShippingLine.Add(new ShippingLineList()
                //        {
                //            ShippingLine = Convert.ToString(Result["ShippingLine"]),
                //            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"])


                //        });
                //    }
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBondReferenceNumberList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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


        public void GetLoadContainerRefNUmber()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GEtListOfLoadedContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            //LoadedContainer ObjLoadedContainer = new LoadedContainer();
            List<LoadContainerReferenceNumberList> LoadContainerReferenceLst = new List<LoadContainerReferenceNumberList>();

            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LoadContainerReferenceLst.Add(new LoadContainerReferenceNumberList
                    {
                        LoadContReqId = int.Parse(Convert.ToString(Result["LoadContReqId"])),
                        LoadContReqNo = Convert.ToString(Result["LoadContReqNo"]),
                        LoadContReqDate = Convert.ToString(Result["LoadContReqDate"])
                    });

                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjLoadedContainer.listShippingLine.Add(new ShippingLineList()
                //        {
                //            ShippingLine = Convert.ToString(Result["ShippingLine"]),
                //            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"])


                //        });
                //    }
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LoadContainerReferenceLst;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetShippingLineLoadCont()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ShippingLineList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            //LoadedContainer ObjLoadedContainer = new LoadedContainer();
            List<ShippingLineList> ShippingLineLst = new List<ShippingLineList>();

            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ShippingLineLst.Add(new ShippingLineList
                    {
                        ShippingLine = Convert.ToString(Result["ShippingLine"]),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"])
                    });

                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjLoadedContainer.listShippingLine.Add(new ShippingLineList()
                //        {
                //            ShippingLine = Convert.ToString(Result["ShippingLine"]),
                //            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"])


                //        });
                //    }
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ShippingLineLst;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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


        public void AddEditEntryThroughGate(ChnEntryThroughGate ObjEntryThroughGate)
        {
            //if(ObjEntryThroughGate.EntryDateTime != null && ObjEntryThroughGate.EntryDateTime != "")
            //{ }
            //if (ObjEntryThroughGate.ReferenceDate != null && ObjEntryThroughGate.ReferenceDate != "")
            //{ }
            //DateTime EntryDateTm = DateTime.ParseExact(ObjEntryThroughGate.EntryDateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime EntryDateTm =DateTime. ObjEntryThroughGate.EntryDateTime;
            DateTime? ReferenceDt = null;
            var Entrydt = (dynamic)null;
            var terminalDate = (dynamic)null;
            var SysDateTime = (dynamic)null;
            if (ObjEntryThroughGate.ReferenceDate != null && ObjEntryThroughGate.ReferenceDate != "")
            {
                ReferenceDt = Convert.ToDateTime(ObjEntryThroughGate.ReferenceDate);
            }
            if (ObjEntryThroughGate.EntryDateTime != null && ObjEntryThroughGate.EntryDateTime != "")
            {
                Entrydt = DateTime.ParseExact(ObjEntryThroughGate.EntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            }
            if (ObjEntryThroughGate.TerminalOutDateTime != null && ObjEntryThroughGate.TerminalOutDateTime != "")
            {
                terminalDate = DateTime.ParseExact(ObjEntryThroughGate.TerminalOutDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            }
            if (ObjEntryThroughGate.SystemDateTime != null && ObjEntryThroughGate.SystemDateTime != "")
            {
                SysDateTime = DateTime.ParseExact(ObjEntryThroughGate.SystemDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            }
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo == null ? "" : ObjEntryThroughGate.ContainerNo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateInNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.GateInNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Entrydt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TerminalDateTime", MySqlDbType = MySqlDbType.DateTime, Value = terminalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ReferenceNo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceDate", MySqlDbType = MySqlDbType.DateTime, Value = ReferenceDt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SystemDateTime", MySqlDbType = MySqlDbType.DateTime, Value = SysDateTime });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Size = 30, Value = ObjEntryThroughGate.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLine", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ShippingLine });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 30, Value = ObjEntryThroughGate.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo1", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo1 == null ? "" : ObjEntryThroughGate.ContainerNo1) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = ObjEntryThroughGate.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Reefer", MySqlDbType = MySqlDbType.Bit, Value = ObjEntryThroughGate.Reefer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ShippingLineSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.VehicleNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChallanNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ChallanNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjEntryThroughGate.CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.NoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.GrossWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TareWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.TareWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositorName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.DepositorName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ContainerType });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.OperationType });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportMode", MySqlDbType = MySqlDbType.Int32, Size = 1, Value = ObjEntryThroughGate.TransportMode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerLoadType", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = ObjEntryThroughGate.ContainerLoadType });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportFrom", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = ObjEntryThroughGate.TransportFrom });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CBT", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.IsCBT ? 1 : 0 });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_TPNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ObjEntryThroughGate.TPNo });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 100, Direction = ParameterDirection.Output });
            
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsOdc", MySqlDbType = MySqlDbType.Bit, Value = ObjEntryThroughGate.IsOdc });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Onwheel", MySqlDbType = MySqlDbType.Int32, Value =Convert.ToInt32(ObjEntryThroughGate.Onwheel) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBlId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjEntryThroughGate.ContainerId) });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditEntryThroughGate", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Entry Through Gate Saved Successfully" : "Entry Through Gate Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Duplicate Container No.";
                }
                else if (Result == -1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Entry Through Gate Could Not Be Saved";
                }
            }
            catch (Exception ex)
            {
                var file = (dynamic)null;
                string Error = ex.InnerException.Message.ToString();
                string CuurDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("/", "");
                string time = DateTime.Now.ToString("H:mm").Replace(":", "");

                string FolderPath2 = HttpContext.Current.Server.MapPath("~/GateError/EntryError/" + CuurDate);
                if (!System.IO.Directory.Exists(FolderPath2))
                {
                    System.IO.Directory.CreateDirectory(FolderPath2);

                }
                file = Path.Combine(FolderPath2, time + "_ErrorEntry.txt");


                using (var tw = new StreamWriter(file, true))
                {
                    tw.WriteLine(Error);
                    tw.Close();
                }

                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }



        public void EntryMailStatus(string containerId, int lastInsertedId)
        {
            //string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_containerId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = containerId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_lastInsertedId", MySqlDbType = MySqlDbType.Int32, Value = lastInsertedId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            // LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("EntryMailStatus", CommandType.StoredProcedure, DParam/*, out GeneratedClientId*/);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Email Of Entry Through Gate Sent Successfully";
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
        public void GetDetailsForGateEntryMail(string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDetailsForGateEntryMail", CommandType.StoredProcedure, DParam);
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
        public void GetEntryThroughGateDt(int EntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditEntryThroughGate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            ChnEntryThroughGate ObjEntryThroughGate = new ChnEntryThroughGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjEntryThroughGate.EntryId = Convert.ToInt32(Result["EntryId"]);
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.CFSCode = Result["CFSCode"].ToString();
                    ObjEntryThroughGate.GateInNo = Result["GateInNo"].ToString();
                    //ObjEntryThroughGate.EntryDateTime = Result["EntryDateTime"].ToString();
                    ObjEntryThroughGate.EntryDateTime = Convert.ToString(Result["EntryDateTime"] == null ? "" : Result["EntryDateTime"]);
                    ObjEntryThroughGate.TerminalOutDateTime = Convert.ToString(Result["TerminalOutDateTime"] == null ? "" : Result["TerminalOutDateTime"]);
                    ObjEntryThroughGate.SystemDateTime = Convert.ToString(Result["SystemDateTime"]);
                    ObjEntryThroughGate.ReferenceNo = Result["ReferenceNo"].ToString();
                    //ObjEntryThroughGate.ReferenceDate= Result["ReferenceDate"].ToString();
                    ObjEntryThroughGate.ReferenceDate = Convert.ToString(Result["ReferenceDate"] == null ? "" : Result["ReferenceDate"]);
                    ObjEntryThroughGate.ShippingLine = Result["ShippingLine"].ToString();
                    ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.ContainerNo1 = (Result["ContainerNo1"] == null ? "" : Result["ContainerNo1"]).ToString();


                    ObjEntryThroughGate.Size = Result["Size"].ToString();
                    //ObjEntryThroughGate.Reefer =Convert.ToBoolean(Result["Reefer"].ToString());

                    ObjEntryThroughGate.Reefer = Convert.ToBoolean(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]);

                    ObjEntryThroughGate.CustomSealNo = Result["CustomSealNo"].ToString();
                    ObjEntryThroughGate.ShippingLineSealNo = Result["ShippingLineSealNo"].ToString();
                    ObjEntryThroughGate.VehicleNo = Result["VehicleNo"].ToString();
                    ObjEntryThroughGate.ChallanNo = Result["ChallanNo"].ToString();

                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    ObjEntryThroughGate.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjEntryThroughGate.ActualPackages = Convert.ToInt32(Result["ActualPackages"] == DBNull.Value ? 0 : Result["ActualPackages"]);

                    ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWeight"].ToString());
                    ObjEntryThroughGate.TareWeight = Convert.ToDecimal(Result["TareWeight"].ToString());
                    ObjEntryThroughGate.DepositorName = Result["DepositorName"].ToString();
                    ObjEntryThroughGate.Remarks = Result["Remarks"].ToString();
                    ObjEntryThroughGate.CargoDescription = Result["CargoDescription"].ToString();
                    ObjEntryThroughGate.ContainerType = Result["ContainerType"].ToString();
                    ObjEntryThroughGate.OperationType = Result["OperationType"].ToString();
                    ObjEntryThroughGate.TransportMode = Convert.ToInt32(Result["TransportMode"].ToString());
                    ObjEntryThroughGate.TransportFrom = Result["TransportFrom"].ToString();
                    ObjEntryThroughGate.ContainerLoadType = Result["ContainerLoadType"].ToString();
                    ObjEntryThroughGate.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    ObjEntryThroughGate.TPNo = Result["TPNo"].ToString();
                    ObjEntryThroughGate.IsCBT = Convert.ToInt32(Result["CBT"]) == 1 ? true : false;
                    ObjEntryThroughGate.IsOdc = Convert.ToInt32(Result["IsOdc"]) == 1 ? true : false;
                    ObjEntryThroughGate.Onwheel = Convert.ToInt32(Result["OnWheel"]) == 1 ? true : false;
                }
               
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
            List<CHN_PickupModel> LstPickUp = new List<CHN_PickupModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPickUp.Add(new CHN_PickupModel
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
        public void AddEditEntryThroughGateVehicle(Dnd_AddExportVehicle ObjEntryThroughGate)
        {
            //if(ObjEntryThroughGate.EntryDateTime != null && ObjEntryThroughGate.EntryDateTime != "")
            //{ }
            //if (ObjEntryThroughGate.ReferenceDate != null && ObjEntryThroughGate.ReferenceDate != "")
            //{ }
            //DateTime EntryDateTm = DateTime.ParseExact(ObjEntryThroughGate.EntryDateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime EntryDateTm =DateTime. ObjEntryThroughGate.EntryDateTime;


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DtlEntryId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.DtlEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ExportCFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ExportReferenceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ExportVehicleNo.ToUpper() });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleEntryDt", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjEntryThroughGate.VehicleEntryDt).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPkg", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.ExportNoOfPkg });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrWt", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.ExportGrWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditGateEntryVehicleDtl", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "";
                }
                else if (Result == 3)
                {

                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Cannot add / edit as stuffing request done.";
                }
                else if (Result == 4)
                {

                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Cannot add / edit as full carting done.";
                }

                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Entry Through Gate Could Not Be Saved";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        #region Vehicle Wise ShipBill Gate Entry
        public void AddEditVehicleWiseShipBillGateEntry(ChnEntryThroughGate ObjEntryThroughGate, string XML)
        {
            //if(ObjEntryThroughGate.EntryDateTime != null && ObjEntryThroughGate.EntryDateTime != "")
            //{ }
            //if (ObjEntryThroughGate.ReferenceDate != null && ObjEntryThroughGate.ReferenceDate != "")
            //{ }
            //DateTime EntryDateTm = DateTime.ParseExact(ObjEntryThroughGate.EntryDateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime EntryDateTm =DateTime. ObjEntryThroughGate.EntryDateTime;
            DateTime? ReferenceDt = null;
            var Entrydt = (dynamic)null;
            var SysDateTime = (dynamic)null;
            if (ObjEntryThroughGate.ReferenceDate != null && ObjEntryThroughGate.ReferenceDate != "")
            {
                ReferenceDt = Convert.ToDateTime(ObjEntryThroughGate.ReferenceDate);
            }
            if (ObjEntryThroughGate.EntryDateTime != null && ObjEntryThroughGate.EntryDateTime != "")
            {
                Entrydt = DateTime.ParseExact(ObjEntryThroughGate.EntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            }

            if (ObjEntryThroughGate.SystemDateTime != null && ObjEntryThroughGate.SystemDateTime != "")
            {
                SysDateTime = DateTime.ParseExact(ObjEntryThroughGate.SystemDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            }
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo == null ? "" : ObjEntryThroughGate.ContainerNo.Trim()) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateInNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.GateInNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Entrydt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ReferenceNo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceDate", MySqlDbType = MySqlDbType.DateTime, Value = ReferenceDt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SystemDateTime", MySqlDbType = MySqlDbType.DateTime, Value = SysDateTime });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Size = 30, Value = ObjEntryThroughGate.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLine", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ShippingLine });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 30, Value = ObjEntryThroughGate.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo1", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo1 == null ? "" : ObjEntryThroughGate.ContainerNo1.Trim()) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = ObjEntryThroughGate.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Reefer", MySqlDbType = MySqlDbType.Bit, Value = ObjEntryThroughGate.Reefer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ShippingLineSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.VehicleNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChallanNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ChallanNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjEntryThroughGate.CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.NoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ActualPackages", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.ActualPackages });

            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.GrossWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TareWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.TareWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositorName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.DepositorName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ContainerType });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.OperationType });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportMode", MySqlDbType = MySqlDbType.Int32, Size = 1, Value = ObjEntryThroughGate.TransportMode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerLoadType", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = ObjEntryThroughGate.ContainerLoadType });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportFrom", MySqlDbType = MySqlDbType.VarChar, Value = ObjEntryThroughGate.TransportFrom });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CBT", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.IsCBT ? 1 : 0 });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_TPNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ObjEntryThroughGate.TPNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = XML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 100, Direction = ParameterDirection.Output });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditVehicleWiseShipBillGateentry", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Entry Through Gate Saved Successfully" : "Entry Through Gate Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Duplicate Container No.";
                }
                else if (Result == -1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Entry Through Gate Could Not Be Saved";
                }
            }
            catch (Exception ex)
            {
                var file = (dynamic)null;
                string Error = ex.InnerException.Message.ToString();
                string CuurDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("/", "");
                string time = DateTime.Now.ToString("H:mm").Replace(":", "");

                string FolderPath2 = HttpContext.Current.Server.MapPath("~/GateError/EntryError/" + CuurDate);
                if (!System.IO.Directory.Exists(FolderPath2))
                {
                    System.IO.Directory.CreateDirectory(FolderPath2);

                }
                file = Path.Combine(FolderPath2, time + "_ErrorEntry.txt");


                using (var tw = new StreamWriter(file, true))
                {
                    tw.WriteLine(Error);
                    tw.Close();
                }

                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void GetVehicleWiseReferenceNumber()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReferenceNumberExportLoadedVehicle", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();

            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjReferenceNumber.ReferenceList.Add(new CCINList
                    {
                        CCINId = int.Parse(Convert.ToString(Result["Id"])),
                        CCINNo = Convert.ToString(Result["CCINNo"]),
                        CCINDate = Convert.ToString(Result["CCINDate"]),
                        ShippingLineId = int.Parse(Convert.ToString(Result["ShippingLineId"])),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        NoOfUnits = Convert.ToString(Result["Package"]),
                        Weight = Convert.ToString(Result["Weight"]),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        CHAId = int.Parse(Convert.ToString(Result["CHAId"])),
                        CHAName = Result["CHA"].ToString(),
                    });

                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjReferenceNumber.listExport.Add(new GateEntryExport()
                //        {
                //            ReferenceNo = Convert.ToString(Result["ReferenceNo"]),
                //            ReferenceDate = Convert.ToString(Result["ReferenceDate"]),
                //            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                //            CargoDescription = Convert.ToString(Result["CargoDescription"])

                //        });
                //    }
                //}
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        ObjReferenceNumber.listShippingLine.Add(new ShippingLineList()
                        {
                            ShippingLine = Convert.ToString(Result["ShippingLine"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"])


                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjReferenceNumber;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
        public void GetEntryThroughGateVehicleWiseCCINDt(int EntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditVehicleWiseSBillGtEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            ChnEntryThroughGate ObjEntryThroughGate = new ChnEntryThroughGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjEntryThroughGate.EntryId = Convert.ToInt32(Result["EntryId"]);
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.CFSCode = Result["CFSCode"].ToString();
                    ObjEntryThroughGate.GateInNo = Result["GateInNo"].ToString();
                    //ObjEntryThroughGate.EntryDateTime = Result["EntryDateTime"].ToString();
                    ObjEntryThroughGate.EntryDateTime = Convert.ToString(Result["EntryDateTime"] == null ? "" : Result["EntryDateTime"]);
                    ObjEntryThroughGate.SystemDateTime = Convert.ToString(Result["SystemDateTime"]);
                    ObjEntryThroughGate.ReferenceNo = Result["ReferenceNo"].ToString();
                    //ObjEntryThroughGate.ReferenceDate= Result["ReferenceDate"].ToString();
                    ObjEntryThroughGate.ReferenceDate = Convert.ToString(Result["ReferenceDate"] == null ? "" : Result["ReferenceDate"]);
                    ObjEntryThroughGate.ShippingLine = Result["ShippingLine"].ToString();
                    ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.CHAId= Convert.ToInt32(Result["CHAId"]);
                    ObjEntryThroughGate.ContainerNo1 = (Result["ContainerNo1"] == null ? "" : Result["ContainerNo1"]).ToString();


                    ObjEntryThroughGate.Size = Result["Size"].ToString();
                    //ObjEntryThroughGate.Reefer =Convert.ToBoolean(Result["Reefer"].ToString());

                    ObjEntryThroughGate.Reefer = Convert.ToBoolean(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]);

                    ObjEntryThroughGate.CustomSealNo = Result["CustomSealNo"].ToString();
                    ObjEntryThroughGate.ShippingLineSealNo = Result["ShippingLineSealNo"].ToString();
                    ObjEntryThroughGate.VehicleNo = Result["VehicleNo"].ToString();
                    ObjEntryThroughGate.ChallanNo = Result["ChallanNo"].ToString();


                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    ObjEntryThroughGate.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjEntryThroughGate.ActualPackages = Convert.ToInt32(Result["ActualPackages"] == DBNull.Value ? 0 : Result["ActualPackages"]);

                    ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWeight"].ToString());
                    ObjEntryThroughGate.TareWeight = Convert.ToDecimal(Result["TareWeight"].ToString());
                    ObjEntryThroughGate.DepositorName = Result["DepositorName"].ToString();
                    ObjEntryThroughGate.Remarks = Result["Remarks"].ToString();
                    ObjEntryThroughGate.CargoDescription = Result["CargoDescription"].ToString();
                    ObjEntryThroughGate.ContainerType = Result["ContainerType"].ToString();
                    ObjEntryThroughGate.OperationType = Result["OperationType"].ToString();
                    ObjEntryThroughGate.TransportMode = Convert.ToInt32(Result["TransportMode"].ToString());
                    ObjEntryThroughGate.TransportFrom = Result["TransportFrom"].ToString();
                    ObjEntryThroughGate.ContainerLoadType = Result["ContainerLoadType"].ToString();
                    ObjEntryThroughGate.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    ObjEntryThroughGate.TPNo = Result["TPNo"].ToString();
                    ObjEntryThroughGate.IsCBT = Convert.ToInt32(Result["CBT"]) == 1 ? true : false;
                    ObjEntryThroughGate.IsOdc = Convert.ToInt32(Result["IsOdc"]) == 1 ? true : false;
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjEntryThroughGate.listAddRef.Add(new CHN_AddMoreRefForCCIN
                        {
                            addRefNo = Convert.ToString(Result["ReferenceNo"]),
                            addRefpkg = Convert.ToInt32(Result["Actual_Pkg"]),

                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
        public void GetAllEntryThroughVehicleWiseShipbillListPage(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = null });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetVehicleWiseShipBillListPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ChnEntryThroughGate> LstEntryThroughGate = new List<ChnEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new ChnEntryThroughGate
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
        public void DeleteVehicleWiseSBGateEntry(int EntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("DeleteVehicleWiseSBGateEntry", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Gate Entry Deleted Successfully";
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete As It Is Used In Another Form";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete As It's Custom Checking has Been Done";
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

        #region Gate Exit For Factory Stuffing

        public void GetFSGateExitTime()
        {
            int Status = 0;

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDateTime", CommandType.StoredProcedure, null);
            _DBResponse = new DatabaseResponse();
            DSRGateExitFactoryStuffing objGateExitFactoryStuffing = new DSRGateExitFactoryStuffing();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objGateExitFactoryStuffing.GateExitDateTime = Result["CurrentDate"].ToString();
                    objGateExitFactoryStuffing.Time = Result["CurrentTime"].ToString();


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objGateExitFactoryStuffing;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
        public void GetFactoryStuffingRequestNoLst()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetFactoryStuffingRequestNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRFSRequestNoList> lstRequestNo = new List<DSRFSRequestNoList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstRequestNo.Add(new DSRFSRequestNoList
                    {
                        FSRequestId = Convert.ToInt32(Result["LoadContReqId"].ToString()),
                        FSRequestNo = Result["LoadContReqNo"].ToString(),
                        FSRequestDate = Result["LoadContReqDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstRequestNo;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
        public void ContainerForGateExitFS(int FSRequestId)
        {

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            LstParam.Add(new MySqlParameter { ParameterName = "in_FSRequestId", MySqlDbType = MySqlDbType.VarChar, Value = FSRequestId });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ContainerDetailsForFactoryStuffing", CommandType.StoredProcedure, DParam);
            IList<DSRcontainerExitFS> lstcontainerAgainstFS = new List<DSRcontainerExitFS>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    lstcontainerAgainstFS.Add(new DSRcontainerExitFS
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),

                        //IsReefer = Convert.ToInt32(Result["Reefer"].ToString()),

                        size = Result["size"].ToString() == null ? "" : Result["size"].ToString(),
                        //CargoDescription = Result["CargoDescription"].ToString(),
                        //CargoType = Result["CargoType"].ToString(),
                        //NoOfUnits = Result["NoOfUnits"].ToString(),
                        //VehicleNo = Result["VehicleNo"].ToString(),
                        //Weight = Result["GrossWt"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        ShippingLineId = Result["ShippingLineId"].ToString(),
                        OperationType = Result["OperationType"].ToString(),

                        CHAName = Result["CHAName"].ToString(),
                        FSRequestNo = Result["LoadContReqNo"].ToString(),
                        FSRequestDate = Result["LoadContReqDate"].ToString(),


                    });
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstcontainerAgainstFS;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void AddEditGateExitFactoryStuffing(DSRGateExitFactoryStuffing ObjGateExitFS, int Uid)
        {

            ObjGateExitFS.Uid = Uid;
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExitIdHeader", MySqlDbType = MySqlDbType.Int32, Value = ObjGateExitFS.ExitIdHeader });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateExitNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjGateExitFS.GateExitNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FSRequestNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjGateExitFS.FSRequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FSRequestId", MySqlDbType = MySqlDbType.Int32, Value = ObjGateExitFS.FSRequestId });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.Int32, Value = int.Parse(ObjExitThroughGateHeader.CFSCode) });*/

            LstParam.Add(new MySqlParameter { ParameterName = "in_GateExitDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjGateExitFS.GateExitDateTime).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FSRequestDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjGateExitFS.FSRequestDate).ToString("yyyy-MM-dd HH:mm:ss") });

            LstParam.Add(new MySqlParameter { ParameterName = "in_StrExitThroughGateDetails", MySqlDbType = MySqlDbType.Text, Value = ObjGateExitFS.StrExitThroughGateDetails });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjGateExitFS.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjGateExitFS.BranchId });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ObjGateExitFS.ExitIdHeader, Direction = ParameterDirection.Output });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditGateExitForFactoryStuffing", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Exit Through Gate Saved Successfully" : "Exit Through Gate Updated Successfully";
                }

                else if (Result == 3)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Exit Date should be less than or equal to PickUp Validity Date";
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

        public void GetAllGateExitFactoryStuffingList(int page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExitIdHeader", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllGateExitFactoryStuffingHdrList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRGateExitFactoryStuffing> LstExitThroughGate = new List<DSRGateExitFactoryStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstExitThroughGate.Add(new DSRGateExitFactoryStuffing
                    {
                        GateExitNo = Result["GateExitNo"].ToString(),
                        GateExitDateTime = Result["GateExitDateTime"].ToString(),
                        FSRequestNo = Result["FSRequestNo"].ToString(),
                        ExitIdHeader = Convert.ToInt32(Result["ExitIdHeader"]),
                        ContainerNo = Convert.ToString((Result["ContainerNo"]))
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
        public void GetAllGateExitFactoryStuffingList(string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllGateExitFactoryStuffingSearchByContainerNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRGateExitFactoryStuffing> LstExitThroughGate = new List<DSRGateExitFactoryStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstExitThroughGate.Add(new DSRGateExitFactoryStuffing
                    {
                        GateExitNo = Result["GateExitNo"].ToString(),
                        GateExitDateTime = Result["GateExitDateTime"].ToString(),
                        FSRequestNo = Result["FSRequestNo"].ToString(),
                        ExitIdHeader = Convert.ToInt32(Result["ExitIdHeader"]),
                        ContainerNo = Convert.ToString((Result["ContainerNo"]))
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

        public void GetGateExitFactoryStuffing(int ExitIdHdr)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExitIdHdr", MySqlDbType = MySqlDbType.Int32, Value = ExitIdHdr });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditGateExitFactoryStuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRGateExitFactoryStuffing ObjExitThroughGateHeader = new DSRGateExitFactoryStuffing();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjExitThroughGateHeader.ExitIdHeader = Convert.ToInt32(Result["ExitIdHeader"]);
                    ObjExitThroughGateHeader.GateExitNo = Result["GateExitNo"].ToString();
                    ObjExitThroughGateHeader.FSRequestNo = Result["FSRequestNo"].ToString();//
                    ObjExitThroughGateHeader.FSRequestId = Convert.ToInt32((Result["FSRequestId"] == DBNull.Value ? 0 : Result["FSRequestId"]).ToString());
                    ObjExitThroughGateHeader.GateExitDateTime = Result["GateExitDateTime"].ToString();
                    ObjExitThroughGateHeader.FSRequestDate = Result["FSRequestDate"].ToString();

                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                            ObjExitThroughGateHeader.containerList.Add(new DSRcontainerExitFS()
                            {
                                ExitIdHeader = Convert.ToInt32(Result["ExitIdHeader"]),
                                ExitIdDtls = Convert.ToInt32(Result["ExitIdDtls"]),
                                ContainerNo = Result["ContainerNo"].ToString(),
                                size = Result["size"].ToString() == null ? "" : Result["size"].ToString(),
                                VehicleNo = Result["VehicleNo"].ToString(),
                                CFSCode = Result["CFSCode"].ToString(),
                                ShippingLine = Result["ShippingLine"].ToString(),
                                ShippingLineId = Result["ShippingLineId"].ToString(),
                                CHAName = Result["CHAName"].ToString(),
                                DepositorName = Result["DepositorName"].ToString(),
                                Remarks = Result["Remarks"].ToString(),

                            });
                        }
                    }


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

        public void DeleteGateExitFactoryStuffing(int ExitId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExitId", MySqlDbType = MySqlDbType.Int32, Value = ExitId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteGateExitFactoryStuffing", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Record Of Gate Exit Factory Stuffing Deleted Successfully";
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


        #region Entry Through Gate Factory Destuffing Empty

        public void GetContainerByRoadFDEmpty()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainerByRoadFDEmpty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<container> Lstcontainer = new List<container>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstcontainer.Add(new container
                    {
                        ContainerName = Result["ContainerNo"].ToString(),


                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lstcontainer;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetFDEmptyContainerDetails(string ContainerNumber, string CFSCode)
        {
            ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllFDEmptyContainerDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSREntryThroughGate ObjEntryThroughGate = new DSREntryThroughGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjEntryThroughGate.ShippingLine = Result["ShippingLineName"].ToString();
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.ContainerNo1 = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.Size = Result["ContainerSize"].ToString();
                    ObjEntryThroughGate.CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"]);
                    ObjEntryThroughGate.EntryDateTime = Convert.ToDateTime(Result["EntryDateTime"]).ToString("dd/MM/yyyy");
                    ObjEntryThroughGate.EntryTime = Result["Entime"].ToString();
                    ObjEntryThroughGate.IsOdc = Convert.ToInt32(Result["Ct_ODC"]);
                    ObjEntryThroughGate.TransportFrom = 0;
                    ObjEntryThroughGate.ContainerLoadType = Result["ContainerLoadType"].ToString();
                    ObjEntryThroughGate.ShippingLineSealNo = Result["ShippingLineNo"].ToString();
                    ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjEntryThroughGate.EntryTime = ObjEntryThroughGate.EntryTime;
                    ObjEntryThroughGate.Movement = Result["MovementType"].ToString();


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
        public void AddEditEntryThroughGateFDEmpty(DSREntryThroughGate ObjEntryThroughGate)
        {

            //DateTime? ReferenceDt = null;

            var Entrydt = (dynamic)null;
            var SysDateTime = (dynamic)null;
            //if (ObjEntryThroughGate.ReferenceDate != null && ObjEntryThroughGate.ReferenceDate != "")
            //{
            //    ReferenceDt = Convert.ToDateTime(ObjEntryThroughGate.ReferenceDate);

            //}
            if (ObjEntryThroughGate.EntryDateTime != null && ObjEntryThroughGate.EntryDateTime != "")
            {
                Entrydt = DateTime.ParseExact(ObjEntryThroughGate.EntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            }

            if (ObjEntryThroughGate.SystemDateTime != null && ObjEntryThroughGate.SystemDateTime != "")
            {
                SysDateTime = DateTime.ParseExact(ObjEntryThroughGate.SystemDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            }
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo == null ? "" : ObjEntryThroughGate.ContainerNo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateInNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.GateInNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Entrydt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SystemDateTime", MySqlDbType = MySqlDbType.DateTime, Value = SysDateTime });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Size = 30, Value = ObjEntryThroughGate.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLine", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ShippingLine });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 30, Value = ObjEntryThroughGate.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo1", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo1 == null ? "" : ObjEntryThroughGate.ContainerNo1) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = ObjEntryThroughGate.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.VehicleNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.Movement });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjEntryThroughGate.CargoType });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ContainerType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.OperationType });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportMode", MySqlDbType = MySqlDbType.Int32, Size = 1, Value = ObjEntryThroughGate.TransportMode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerLoadType", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = ObjEntryThroughGate.ContainerLoadType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Odc", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.IsOdc });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OCFSCode", MySqlDbType = MySqlDbType.VarChar, Value = (ObjEntryThroughGate.OCFSCode == null ? "" : ObjEntryThroughGate.OCFSCode) });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 100, Direction = ParameterDirection.Output });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditEntryThroughGateFDEmpty", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Entry Through Gate Saved Successfully" : "Entry Through Gate Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Duplicate Container No.";
                }
                else if (Result == -1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Entry Through Gate Could Not Be Saved";
                }
            }
            catch (Exception ex)
            {
                var file = (dynamic)null;
                string Error = ex.InnerException.Message.ToString();
                string CuurDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("/", "");
                string time = DateTime.Now.ToString("H:mm").Replace(":", "");

                string FolderPath2 = HttpContext.Current.Server.MapPath("~/GateError/EntryError/" + CuurDate);
                if (!System.IO.Directory.Exists(FolderPath2))
                {
                    System.IO.Directory.CreateDirectory(FolderPath2);

                }
                file = Path.Combine(FolderPath2, time + "_ErrorEntry.txt");


                using (var tw = new StreamWriter(file, true))
                {
                    tw.WriteLine(Error);
                    tw.Close();
                }

                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void GetAllEntryThroughGateFDEmptyListPage(int Page, string sText)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchText", MySqlDbType = MySqlDbType.VarChar, Value = sText });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGateFDEmptyListPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSREntryThroughGate> LstEntryThroughGate = new List<DSREntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new DSREntryThroughGate
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["FSEntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Movement = Result["MovementType"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void GetEntryThroughGateFDEmpty(int EntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditEntryThroughGateFDEmpty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSREntryThroughGate ObjEntryThroughGate = new DSREntryThroughGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjEntryThroughGate.EntryId = Convert.ToInt32(Result["EntryId"]);
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.CFSCode = Result["CFSCode"].ToString();
                    ObjEntryThroughGate.GateInNo = Result["GateInNo"].ToString();
                    ObjEntryThroughGate.EntryDateTime = Convert.ToString(Result["EntryDateTime"] == null ? "" : Result["EntryDateTime"]);
                    ObjEntryThroughGate.SystemDateTime = Convert.ToString(Result["SystemDateTime"]);
                    ObjEntryThroughGate.ShippingLine = Result["ShippingLine"].ToString();
                    ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjEntryThroughGate.ContainerNo1 = (Result["ContainerNo1"] == null ? "" : Result["ContainerNo1"]).ToString();
                    ObjEntryThroughGate.Size = Result["Size"].ToString();
                    ObjEntryThroughGate.VehicleNo = Result["VehicleNo"].ToString();
                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    ObjEntryThroughGate.Remarks = Result["Remarks"].ToString();
                    ObjEntryThroughGate.ContainerType = Result["ContainerType"].ToString();
                    ObjEntryThroughGate.OperationType = Result["OperationType"].ToString();
                    ObjEntryThroughGate.TransportMode = Convert.ToInt32(Result["TransportMode"].ToString());
                    ObjEntryThroughGate.ContainerLoadType = Result["ContainerLoadType"].ToString();
                    ObjEntryThroughGate.IsOdc = Convert.ToInt32(Result["IsOdc"]);
                    ObjEntryThroughGate.Movement = Result["MovementType"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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

        public void DeleteEntryThroughGateFDEmpty(int EntryId)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Size = 100, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteEnrtyThroughGateFDEmpty", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Record Of Gate Entry Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = GeneratedClientId;
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Record Of Entry Through Gate Could Not Be Deleted";
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

        public void GetPortOfLoading()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortOfLoading", CommandType.StoredProcedure);
            List<SelectListItem> LstPort = new List<SelectListItem>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new SelectListItem { Text = Result["PortName"].ToString(), Value = Result["PortId"].ToString() });
                    //LstPort.Add(new Port
                    //{
                    //    PortName = Convert.ToString(Result["PortName"]),
                    //    PortId = Convert.ToInt32(Result["PortId"]),
                    //});
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
        #endregion
    }
}
