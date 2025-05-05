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
    public class HDB_EntryThroughGateRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void AddEditEntryThroughGate(HDB_EntryThroughGate ObjEntryThroughGate)
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
            if (ObjEntryThroughGate.LCLFCL == null)
            {
                ObjEntryThroughGate.LCLFCL = "";
            }
            if (ObjEntryThroughGate.ReferenceDate != null && ObjEntryThroughGate.ReferenceDate != "")
            {
                ReferenceDt = DateTime.ParseExact(ObjEntryThroughGate.ReferenceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ForwarderName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.ForwarderName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo1", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo1 == null ? "" : ObjEntryThroughGate.ContainerNo1) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = ObjEntryThroughGate.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Reefer", MySqlDbType = MySqlDbType.Bit, Value = ObjEntryThroughGate.Reefer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ShippingLineSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.VehicleNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChallanNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ChallanNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.Purpose });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjEntryThroughGate.CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.NoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.GrossWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CBTFrom", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.CBTFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ContainerType });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.OperationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.StuffType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = ObjEntryThroughGate.LCLFCL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportMode", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjEntryThroughGate.TransPortMode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldCfsCode", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = ObjEntryThroughGate.OldCfsCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositorName", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = ObjEntryThroughGate.DepositorName });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditEntryThroughGate", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
                    _DBResponse.Message = " IsODC Mis-Match Occured .";
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

        public void AddEditEntryThroughGateExport(HDB_EntryThroughGate ObjEntryThroughGate)
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
            if (ObjEntryThroughGate.LCLFCL == null)
            {
                ObjEntryThroughGate.LCLFCL = "";
            }
            if (ObjEntryThroughGate.ReferenceDate != null && ObjEntryThroughGate.ReferenceDate != "")
            {
                ReferenceDt = DateTime.ParseExact(ObjEntryThroughGate.ReferenceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ForwarderName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.ForwarderName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo1", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo1 == null ? "" : ObjEntryThroughGate.ContainerNo1) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = ObjEntryThroughGate.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Reefer", MySqlDbType = MySqlDbType.Bit, Value = ObjEntryThroughGate.Reefer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ShippingLineSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.VehicleNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChallanNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ChallanNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.Purpose });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjEntryThroughGate.CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.NoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.GrossWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositorName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.DepositorName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ContainerType });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.OperationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.StuffType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = ObjEntryThroughGate.LCLFCL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldCfsCode", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = ObjEntryThroughGate.OldCfsCode });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditEntryThroughGateExport", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
        public void AddEditEntryThroughGateBond(HDB_EntryThroughGateBond ObjEntryThroughGate)
        {
            DateTime? ReferenceDt = null;
            var Entrydt = (dynamic)null;
            var SysDateTime = (dynamic)null;
            if (ObjEntryThroughGate.ReferenceDate != null && ObjEntryThroughGate.ReferenceDate != "")
            {
                ReferenceDt = DateTime.ParseExact(ObjEntryThroughGate.ReferenceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo == null ? "" : ObjEntryThroughGate.ContainerNo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateInNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.GateInNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Entrydt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceNoId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.ReferenceNoId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ReferenceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceDate", MySqlDbType = MySqlDbType.DateTime, Value = ReferenceDt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SystemDateTime", MySqlDbType = MySqlDbType.DateTime, Value = SysDateTime });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CBT", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjEntryThroughGate.IsCBT) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsODC", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLine", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ForwarderName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo1", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = ObjEntryThroughGate.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Reefer", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.VehicleNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChallanNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ""});
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjEntryThroughGate.CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.NoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.GrossWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CBTFrom", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ContainerType });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.OperationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldCfsCode", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsVehicle", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.IsVehicle });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TypeofPackage", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.TypeofPackage});
            LstParam.Add(new MySqlParameter { ParameterName = "in_Others", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.Others });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImpName", MySqlDbType = MySqlDbType.VarChar,  Value = ObjEntryThroughGate.DepositorName });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditEntryThroughGateBond", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
            HDB_EntryThroughGate objEntryThroughGate = new HDB_EntryThroughGate();
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

        public void GetBondTime()
        {
            int Status = 0;
            //List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //IDataParameter[] DParam = { };
            //DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDateTime", CommandType.StoredProcedure, null);
            _DBResponse = new DatabaseResponse();
            HDB_EntryThroughGateBond objEntryThroughGate = new HDB_EntryThroughGateBond();
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

        public void DeleteEntryThroughGate(int EntryId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            //LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteEnrtyThroughGate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Record Of Entry Through Gate Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Cannot Delete Entry Through Gate As It Exists In Destuffing Entry";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Cannot Delete Entry Through Gate As It Exists In Custom Appraisement Application";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 5)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Cannot Delete Entry Through Gate As It Exists In BTT Container Paymentsheet";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 6)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Cannot Delete Entry Through Gate As It Exists In Carting Register";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 7)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Cannot Delete Entry Through Gate As It Exists In Deposite Application";
                    _DBResponse.Data = GeneratedClientId;
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


        public void GetAutoPopulateData(string ContainerNumber)
        {
            ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNumber });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainerDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            HDB_EntryThroughGate ObjEntryThroughGate = new HDB_EntryThroughGate();
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
                    ObjEntryThroughGate.LCLFCL = Result["LCLFCL"].ToString();
                    //
                    ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    //
                    ObjEntryThroughGate.Size = Result["ContainerSize"].ToString();
                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"]);
                    ObjEntryThroughGate.EntryDateTime = Convert.ToDateTime(Result["EntryDateTime"]).ToString("dd/MM/yyyy");
                    ObjEntryThroughGate.EntryTime = Result["Entime"].ToString();
                    ObjEntryThroughGate.CargoDescription = Result["CargoDesc"].ToString();
                    ObjEntryThroughGate.Reefer = Convert.ToBoolean(Result["Reefer"].ToString());
                    ObjEntryThroughGate.CBT = Convert.ToBoolean(Result["CBT"]);
                    ObjEntryThroughGate.CBTFrom = Convert.ToString(Result["CbtForm"]);
                    ObjEntryThroughGate.IsODC = Convert.ToBoolean(Result["IsODC"]);
                    ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                    ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjEntryThroughGate.NoOfPackages = Convert.ToInt32(Result["NoofPackages"] == DBNull.Value ? 0 : Result["NoofPackages"]);
                    //EntryThroughGate objEntryThroughGate = new EntryThroughGate();
                    // objEntryThroughGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
                    string strDate = ObjEntryThroughGate.ReferenceDate;
                    string[] arrayDate = strDate.Split(' ');
                    ObjEntryThroughGate.ReferenceDate = arrayDate[0];
                    ObjEntryThroughGate.EntryTime = ObjEntryThroughGate.EntryTime;
                    ObjEntryThroughGate.Remarks = Convert.ToString(Result["Remarks"]);
                    ObjEntryThroughGate.DepositorName = Convert.ToString(Result["DepositorName"]);
                    ObjEntryThroughGate.ShippingLineSealNo = Convert.ToString(Result["ShippingLineSealNo"]);
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

        public void GetLoadedContStuffData(int StuffRefId)
        {
            //ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffAppId", MySqlDbType = MySqlDbType.Int32, Value = StuffRefId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("DetailsForStuffContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<HDB_StuffContainerListWithData> lststuffContainerListWithData = new List<HDB_StuffContainerListWithData>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lststuffContainerListWithData.Add(new HDB_StuffContainerListWithData
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ShippingLineId = Result["ShippingLineId"].ToString(),
                        ShippingLineName = Result["shippingLine"].ToString(),
                        CustomSealNo = Result["CustomSealNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        // Reefer = Result["Reefer"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        NoOfUnits = Result["NoOfUnits"].ToString(),
                        CHAName = Convert.ToString(Result["CHAName"] == null ? "" : Result["CHAName"]),
                        ReferenceDate = Convert.ToString(Result["ReferenceDate"] == null ? "" : Result["ReferenceDate"]),
                        DepositorName = Convert.ToString(Result["ExporterName"] == null ? "" : Result["ExporterName"]),
                        OldCfsCode = Result["CFSCode"].ToString(),
                        GrossWt = (Result["GrossWt"] == null ? "" : Result["GrossWt"]).ToString()
                    });


                    //ViewBag.strTime = objEntryThroughGate.EntryTime;



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lststuffContainerListWithData;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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
            IDataReader Result = DataAccess.ExecuteDataReader("DetailsForBTTLoadedContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<HDB_LoadedContainerListWithData> lstLoadedContainerListWithData = new List<HDB_LoadedContainerListWithData>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstLoadedContainerListWithData.Add(new HDB_LoadedContainerListWithData
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ShippingLineId = Result["ShippingLineId"].ToString(),
                        ShippingLineName = Result["shippingLine"].ToString(),
                        CustomSealNo = Result["CustomSealNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        Reefer = Result["Reefer"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        NoOfUnits = Result["NoOfUnits"].ToString(),
                        CHAName = Convert.ToString(Result["CHAName"] == null ? "" : Result["CHAName"]),
                        ReferenceDate = Convert.ToString(Result["ReferenceDate"] == null ? "" : Result["ReferenceDate"]),
                        DepositorName = Convert.ToString(Result["ExporterName"] == null ? "" : Result["ExporterName"]),
                        GrossWt = (Result["GrossWt"] == null ? "" : Result["GrossWt"]).ToString(),
                        OldCfsCode = Result["CFSCode"].ToString(),
                        IsODC=(Result["IsODC"]).ToString()
                       
                      
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

        public void GetLoadedContDat(int LoadContainerRefId)
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
            List<HDB_LoadedContainerListWithData> lstLoadedContainerListWithData = new List<HDB_LoadedContainerListWithData>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstLoadedContainerListWithData.Add(new HDB_LoadedContainerListWithData
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ShippingLineId = Result["ShippingLineId"].ToString(),
                        ShippingLineName = Result["shippingLine"].ToString(),
                        Size = Result["Size"].ToString(),
                        Reefer = Result["Reefer"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        NoOfUnits = Result["NoOfUnits"].ToString(),
                        CHAName = Convert.ToString(Result["CHAName"] == null ? "" : Result["CHAName"]),
                        ReferenceDate = Convert.ToString(Result["ReferenceDate"] == null ? "" : Result["ReferenceDate"]),
                        DepositorName = Convert.ToString(Result["ExporterName"] == null ? "" : Result["ExporterName"]),
                        GrossWt = (Result["GrossWt"] == null ? "" : Result["GrossWt"]).ToString()

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

        public void GetAllEntryThroughGate(int Page, string OperationType = "")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 2, Value = OperationType });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<HDB_EntryThroughGate> LstEntryThroughGate = new List<HDB_EntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new HDB_EntryThroughGate
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        PrintSealCut = Result["PrintSealCut"].ToString(),
                        CBTContainer = Result["CBTContainer"].ToString(),
                        SubCFSCode = Result["SubCFS"].ToString(),
                        StuffType = Result["StuffType"].ToString(),
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetReferenceNumber", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            HDB_ReferenceNumber ObjReferenceNumber = new HDB_ReferenceNumber();

            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjReferenceNumber.ReferenceList.Add(new ReferenceNumberList
                    {
                        CartingAppId = int.Parse(Convert.ToString(Result["CartingAppId"])),
                        CartingNo = Convert.ToString(Result["ReferenceNo"])
                    });

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjReferenceNumber.listExports.Add(new HDB_EntryExport()
                        {
                            ReferenceNo = Convert.ToString(Result["ReferenceNo"]),
                            ReferenceDate = Convert.ToString(Result["ReferenceDate"]),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            CargoDescription = Convert.ToString(Result["CargoDescription"]),
                            CHAName = Convert.ToString(Result["CHA"] == null ? 0 : Result["CHA"]),
                            DepositorName = Convert.ToString(Result["Exporter"] == null ? 0 : Result["Exporter"]),
                            ForwarderName = Convert.ToString(Result["Forwarder"] == null ? 0 : Result["Forwarder"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] == DBNull.Value ? 0 : Result["NoOfUnits"]),
                            Weight = Convert.ToDecimal(Result["Weight"] == DBNull.Value ? 0 : Result["Weight"]),
                            VehicleNo = Convert.ToString(Result["VehicleNo"] == null ? "" : Result["VehicleNo"]),
                            Reefer = Convert.ToInt32(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjReferenceNumber.listShippingLine.Add(new ShippingLineList()
                        {
                            ShippingLine = Convert.ToString(Result["ShippingLine"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"])


                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjReferenceNumber.listChaName.Add(new ChaNameList()
                        {
                            CHAName = Convert.ToString(Result["CHAName"]),
                            CHAId = Convert.ToInt32(Result["CHAId"])


                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjReferenceNumber.listForwarder.Add(new ForwarderList()
                        {
                            ForwarderName = Convert.ToString(Result["ForwarderName"]),
                            ForwarderId = Convert.ToInt32(Result["ForwarderId"])


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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = 0 });
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

        public void SACDetails(int SACId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = SACId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("BondEntryRefNumber", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<dynamic> obj = new List<dynamic>();
            /*dynamic obj = new
            {
                SpaceappId = 0,
                ApplicationNo = "",
                ApplicationDate = "",
                CHAId = 0,
                CHAName = "",
                ImporterId = 0,
                ImporterName = "",
                NoOfUnits = 0,
                Weight = 0.00,
                NatureOfPackages = "",
                Others = "",
                CargoDescription = "",
                GodownId = 0,
                GodownName = ""
            };*/
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    obj.Add(new
                    {
                        SpaceappId = int.Parse(Convert.ToString(Result["SpaceappId"])),
                        ApplicationNo = Convert.ToString(Result["ApplicationNo"]),
                        ApplicationDate = Convert.ToString(Result["ApplicationDate"]),
                        CHAId = int.Parse(Convert.ToString(Result["CHAId"])),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        ImporterId = Convert.ToInt32(Result["ImporterId"]),
                        ImporterName = Convert.ToString(Result["ImporterName"]),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        NatureOfPackages = Convert.ToString(Result["NatureOfPackages"]),
                        Others = Convert.ToString(Result["Others"]),
                        CargoDescription = Convert.ToString(Result["CargoDescription"]),
                        GodownId = int.Parse(Convert.ToString(Result["GodownId"])),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        CargoType=Convert.ToInt32(Result["CargoType"]),
                        CustomSealNo=Result["CustomSealNo"].ToString()
                    });
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

        public void GetLoadContainerRefNUmb()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_Stufftype", MySqlDbType = MySqlDbType.VarChar, Value = stufftype });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GETListOfBTTLoadedContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            HDB_ReferenceNumber ObjReferenceNumber = new HDB_ReferenceNumber();
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
                        LoadContReqNo = Convert.ToString(Result["LoadContReqNo"])
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
        public void GetLoadContainerRefNUmber()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GETListOfLoadedContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            // HDB_ReferenceNumber ObjReferenceNumber = new HDB_ReferenceNumber();
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
                        LoadContReqNo = Convert.ToString(Result["LoadContReqNo"])
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
        public void GetStuffingRefNUmber()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GEtListOfStuffingRequest", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            // HDB_ReferenceNumber ObjReferenceNumber = new HDB_ReferenceNumber();
            //LoadedContainer ObjLoadedContainer = new LoadedContainer();
            List<StuffingReqList> lstStuffing = new List<StuffingReqList>();

            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstStuffing.Add(new StuffingReqList
                    {
                        StuffingReqId = int.Parse(Convert.ToString(Result["StuffingReqId"])),
                        StuffingReqNo = Convert.ToString(Result["StuffingReqNo"])
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
                    _DBResponse.Data = lstStuffing;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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
        public void GetChaNameLoadCont()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "Forwarder" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure, DParam);
            IList<ChaNameList> lstCHA = new List<ChaNameList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new ChaNameList
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


        public void ListOfForwarderLoadCont()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "Forwarder" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
            IList<ForwarderList> lstForwarder = new List<ForwarderList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstForwarder.Add(new ForwarderList
                    {
                        ForwarderId = Convert.ToInt32(result["EximTraderId"]),
                        ForwarderName = result["NameAddress"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstForwarder;
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

        public void GetEntryThroughGate(int EntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditEntryThroughGate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            HDB_EntryThroughGate ObjEntryThroughGate = new HDB_EntryThroughGate();
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
                    ObjEntryThroughGate.ReferenceNo = Result["ReferenceNo"].ToString();
                    //ObjEntryThroughGate.ReferenceDate= Result["ReferenceDate"].ToString();
                    ObjEntryThroughGate.ReferenceDate = Convert.ToString(Result["ReferenceDate"] == null ? "" : Result["ReferenceDate"]);
                    ObjEntryThroughGate.SystemDateTime = Convert.ToString(Result["SystemDateTime"]);
                    ObjEntryThroughGate.CBT = Convert.ToBoolean(Result["CBT"] == DBNull.Value ? 0 : Result["CBT"]);
                    ObjEntryThroughGate.IsODC = Convert.ToBoolean(Result["IsODC"] == DBNull.Value ? 0 : Result["IsODC"]);
                    ObjEntryThroughGate.ShippingLine = Result["ShippingLine"].ToString();
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.ForwarderName = Result["ForwarderName"].ToString();
                    ObjEntryThroughGate.ContainerNo1 = (Result["ContainerNo1"] == null ? "" : Result["ContainerNo1"]).ToString();
                    ObjEntryThroughGate.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjEntryThroughGate.TransPortMode = (Result["TransportMode"] == null ? "" : Result["TransportMode"]).ToString();
                    ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);

                    ObjEntryThroughGate.Size = Result["Size"].ToString();
                    //ObjEntryThroughGate.Reefer =Convert.ToBoolean(Result["Reefer"].ToString());

                    ObjEntryThroughGate.Reefer = Convert.ToBoolean(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]);

                    ObjEntryThroughGate.CustomSealNo = Result["CustomSealNo"].ToString();
                    ObjEntryThroughGate.ShippingLineSealNo = Result["ShippingLineSealNo"].ToString();
                    ObjEntryThroughGate.VehicleNo = Result["VehicleNo"].ToString();
                    ObjEntryThroughGate.ChallanNo = Result["ChallanNo"].ToString();

                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    ObjEntryThroughGate.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWeight"].ToString());
                    ObjEntryThroughGate.DepositorName = Result["DepositorName"].ToString();
                    ObjEntryThroughGate.Remarks = Result["Remarks"].ToString();
                    ObjEntryThroughGate.CargoDescription = Result["CargoDescription"].ToString();
                    ObjEntryThroughGate.CBTFrom = Result["CBTFrom"].ToString();
                    ObjEntryThroughGate.Purpose = Result["Purpose"].ToString();
                    ObjEntryThroughGate.ContainerType = Result["ContainerType"].ToString();
                    ObjEntryThroughGate.OperationType = Result["OperationType"].ToString();
                    ObjEntryThroughGate.StuffType = Result["StuffType"].ToString();

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

        public void GetEntryThroughGateBond(int EntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditEntryThroughGate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            HDB_EntryThroughGateBond ObjEntryThroughGate = new HDB_EntryThroughGateBond();
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
                    ObjEntryThroughGate.ReferenceNo = Result["ReferenceNo"].ToString();
                    ObjEntryThroughGate.ReferenceDate = Convert.ToString(Result["ReferenceDate"] == null ? "" : Result["ReferenceDate"]);
                    ObjEntryThroughGate.SystemDateTime = Convert.ToString(Result["SystemDateTime"]);
                    ObjEntryThroughGate.IsCBT =Convert.ToInt32(Result["CBT"]);
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.Size = Result["Size"].ToString();
                    ObjEntryThroughGate.CustomSealNo = Result["CustomSealNo"].ToString();
                    ObjEntryThroughGate.VehicleNo = Result["VehicleNo"].ToString();
                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    ObjEntryThroughGate.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWeight"].ToString());
                    ObjEntryThroughGate.DepositorName = Result["DepositorName"].ToString();
                    ObjEntryThroughGate.Remarks = Result["Remarks"].ToString();
                    ObjEntryThroughGate.CargoDescription = Result["CargoDescription"].ToString();
                    ObjEntryThroughGate.ContainerType = Result["ContainerType"].ToString();
                    ObjEntryThroughGate.OperationType = Result["OperationType"].ToString();

                    ObjEntryThroughGate.IsVehicle =Convert.ToInt32(Result["IsVehicle"]);
                    ObjEntryThroughGate.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjEntryThroughGate.TypeofPackage = Result["TypeofPackage"].ToString();
                    ObjEntryThroughGate.Others = Result["Others"].ToString();
                    ObjEntryThroughGate.GodownName = Result["GodownName"].ToString();
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
        //public void GetCountry(int CountryId)
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName= "in_CountryId",MySqlDbType=MySqlDbType.Int32,Size=11,Value=CountryId });
        //    IDataParameter [] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetMstCountry", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    Country ObjCountry = new Country();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            ObjCountry.CountryName = Result["CountryName"].ToString();
        //            ObjCountry.CountryId = Convert.ToInt32(Result["CountryId"]);
        //            ObjCountry.CountryAlias = (Result["CountryAlias"]==null?"":Result["CountryAlias"]).ToString();
        //        }
        //        if(Status==1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = ObjCountry;
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "No Data";
        //            _DBResponse.Data = null;
        //        }
        //    }
        //    catch(Exception ex)
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

        public void GetPrintSealCutting(int EntryId, string OperationType, string ContainerType, string StuffType)
        {
            int Status = 0;

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Value = OperationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Value = ContainerType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffType", MySqlDbType = MySqlDbType.VarChar, Value = StuffType });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintSealCutForGateEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<GateEntrySealCutting> LstEntryThroughGate = new List<GateEntrySealCutting>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    if (OperationType == "Import")
                    {
                        LstEntryThroughGate.Add(new GateEntrySealCutting
                        {
                           
                            Importer = Result["Importer"].ToString(),
                            BLNo = Result["BLNo"].ToString(),
                            SealNo = Result["SealNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            CBTFrom = Result["CBTFrom"].ToString(),
                        });
                    }

                    else if (OperationType == "Bond")
                    {
                        LstEntryThroughGate.Add(new GateEntrySealCutting
                        {
                            CompanyAddress = Result["CompanyAddress"].ToString(),
                            CompanyEmail = Result["CompanyEmail"].ToString(),
                            Importer = Result["Importer"].ToString(),
                            CHA = Result["CHA"].ToString(),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            NoOfUnits = Result["NoOfUnits"].ToString(),
                            SACNo = Result["SacNo"].ToString(),
                            SACDate = Result["SacDate"].ToString(),
                            AWBNo = Result["AWBNo"].ToString(),
                            AWBDate = Result["AWBDate"].ToString(),
                            GodownNo = Result["GodownNo"].ToString(),
                            BLNo = Result["BLNo"].ToString(),
                            SealNo = Result["SealNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                        });
                    }

                    else if (OperationType == "Empty")
                    {
                        LstEntryThroughGate.Add(new GateEntrySealCutting
                        {
                            Importer = Result["Importer"].ToString(),
                            BLNo = Result["BLNo"].ToString(),
                            SealNo = Result["SealNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                            EntryDate = Result["EntryDate"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                        });
                    }




                    else if (OperationType == "Export" && ContainerType == "Empty")
                    {
                        LstEntryThroughGate.Add(new GateEntrySealCutting
                        {

                            BLNo = Result["BLNo"].ToString(),
                            SealNo = Result["SealNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                            Purpose = Result["Purpose"].ToString(),
                            VehicleNo = Result["VehicleNo"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            EntryDate = Result["EntryDate"].ToString(),
                            CHA = Result["CHA"].ToString()
                        });
                    }
                    else if (StuffType == "FactoryStuff")
                    {
                        LstEntryThroughGate.Add(new GateEntrySealCutting
                        {
                            NoOfUnits = Convert.ToString(Result["NoOfUnits"]),
                            //CargoDescription = Result["CargoDescription"].ToString(),
                            ReferenceNo = Result["ReferenceNo"].ToString(),
                            CHA = Result["CHA"].ToString(),
                            Exporter = Result["Exporter"].ToString(),
                            Size = Result["Size"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            VehicleNo = Result["VehicleNo"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            Purpose = Result["Purpose"].ToString(),
                            EntryDate = Result["EntryDate"].ToString()
                            // GodownNo = Result["GodownNo"].ToString(),
                        });
                    }
                    else if (StuffType == "ContainerStuff")
                    {
                        LstEntryThroughGate.Add(new GateEntrySealCutting
                        {
                            NoOfUnits = Convert.ToString(Result["NoOfUnits"]),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            ReferenceNo = Result["ReferenceNo"].ToString(),
                            CHA = Result["CHA"].ToString(),
                            Exporter = Result["Exporter"].ToString(),
                            Size = Result["Size"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            VehicleNo = Result["VehicleNo"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            Purpose = Result["Purpose"].ToString(),
                            EntryDate = Result["EntryDate"].ToString()
                            // GodownNo = Result["GodownNo"].ToString(),
                        });
                    }
                    else if (OperationType == "Export" && ContainerType == "Loaded")
                    {
                        LstEntryThroughGate.Add(new GateEntrySealCutting
                        {
                            NoOfUnits = Convert.ToString(Result["NoOfUnits"]),
                            //CargoDescription = Result["CargoDescription"].ToString(),
                            ReferenceNo = Result["ReferenceNo"].ToString(),
                            CHA = Result["CHA"].ToString(),
                            Exporter = Result["Exporter"].ToString(),
                            Size = Result["Size"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            VehicleNo = Result["VehicleNo"].ToString(),
                            Purpose = Result["Purpose"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            EntryDate = Result["EntryDate"].ToString()
                            //Purpose = Result["Purpose"].ToString(),
                            // GodownNo = Result["GodownNo"].ToString(),
                        });
                    }
                    else
                    {
                        LstEntryThroughGate.Add(new GateEntrySealCutting
                        {
                            NoOfUnits = Convert.ToString(Result["NoOfUnits"]),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            ReferenceNo = Result["ReferenceNo"].ToString(),
                            CHA = Result["CHA"].ToString(),
                            Exporter = Result["Exporter"].ToString(),
                            Size = Result["Size"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            VehicleNo = Result["VehicleNo"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            EntryDate = Result["EntryDate"].ToString(),
                            Purpose = Result["Purpose"].ToString(),
                            GodownNo = Result["GodownNo"].ToString()
                        });
                    }

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

        public void GetBondSpaceDetailsById(int RefId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAppId", MySqlDbType = MySqlDbType.Int32, Value = RefId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSpaceDetailsById", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            HDB_EntryThroughGate ObjEntryThroughGate = new HDB_EntryThroughGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjEntryThroughGate.ReferenceNo = Result["ApplicationNo"].ToString();
                    ObjEntryThroughGate.ReferenceDate = Convert.ToString(Result["ApplicationDate"] == null ? "" : Result["ApplicationDate"]);

                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.ShippingLineId = 0;

                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["NatureOfMaterial"] == DBNull.Value ? 0 : Result["NatureOfMaterial"]);
                    ObjEntryThroughGate.NoOfPackages = Convert.ToInt32(Result["NoOfUnits"] == DBNull.Value ? 0 : Result["NoOfUnits"]);
                    ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["Weight"].ToString());
                    ObjEntryThroughGate.CargoDescription = Result["CargoDescription"].ToString();



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
    }
}