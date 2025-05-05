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
    public class Wlj_EntryThroughGateRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void AddEditEntryThroughGateCBT(WljEntryThroughGate ObjEntryThroughGate, string XML, string ContClass)
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
            string refno = "";
            /*
         int refpos = 0;

         refpos = ObjEntryThroughGate.ReferenceNo.IndexOf("-");
         if (refpos > 0)
             refno = ObjEntryThroughGate.ReferenceNo.Substring(0, refpos);
         else 
         */
            refno = ObjEntryThroughGate.ReferenceNo;

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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo == null ? "" : ObjEntryThroughGate.ContainerNo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateInNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.GateInNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Entrydt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = refno });

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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ActualPackages", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.ActualPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 30, Value = ObjEntryThroughGate.GodownId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.GrossWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TareWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.TareWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositorName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.DepositorName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ContainerType });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.OperationType });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportMode", MySqlDbType = MySqlDbType.Int32, Size = 1, Value = ObjEntryThroughGate.TransportMode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerLoadType", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = ObjEntryThroughGate.ContainerLoadType });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportFrom", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = ObjEntryThroughGate.TransportFrom });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CBT", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.IsCBT ? 1 : 0 });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_TPNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ObjEntryThroughGate.TPNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContClass", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ContClass });
            LstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = XML });

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


        public void AddEditEntryThroughGate(WljEntryThroughGate ObjEntryThroughGate)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo == null ? "" : ObjEntryThroughGate.ContainerNo) });
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_SCMTRXML", MySqlDbType = MySqlDbType.LongText, Value = ObjEntryThroughGate.SCMTRXML });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 100, Direction = ParameterDirection.Output });

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
            WljEntryThroughGate objEntryThroughGate = new WljEntryThroughGate();
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
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Size = 100, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteEntryThroughGate", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
                    _DBResponse.Status = 3;
                    //_DBResponse.Message = "This Invoice could not be deleted. Because other invoice(s) already generated after this invoice.";
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
            WljEntryThroughGate ObjEntryThroughGate = new WljEntryThroughGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjEntryThroughGate.ReferenceNo = Result["ReferenceNo"].ToString();
                    ObjEntryThroughGate.ReferenceDate = Convert.ToDateTime(Result["ReferenceDate"]).ToString("dd/MM/yyyy");
                    ObjEntryThroughGate.ShippingLine = Result["ShippingLineName"].ToString();
                    //ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.ContainerNo1 = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.Size = Result["ContainerSize"].ToString();
                   // ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"]);
                    ObjEntryThroughGate.EntryDateTime = Convert.ToDateTime(Result["EntryDateTime"]).ToString("dd/MM/yyyy");
                    ObjEntryThroughGate.EntryTime = Result["Entime"].ToString();
                   // ObjEntryThroughGate.CargoDescription = Result["CargoDesc"].ToString();
                   // ObjEntryThroughGate.Reefer = Convert.ToBoolean(Result["Reefer"].ToString());
                    //if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 1)
                    //{
                    //    ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                    //}
                    //else if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 3)
                    //{
                       // ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                        //ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWt"]);
                        ObjEntryThroughGate.ContainerLoadType = Result["LoadType"].ToString();
                        ObjEntryThroughGate.ShippingLineSealNo = Result["SLineNo"].ToString();
                        ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                   // }
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
            WljEntryThroughGate ObjEntryThroughGate = new WljEntryThroughGate();
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
            List<WFLDLoadedContainerListWithData> lstLoadedContainerListWithData = new List<WFLDLoadedContainerListWithData>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstLoadedContainerListWithData.Add(new WFLDLoadedContainerListWithData
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ShippingLineId = Result["ShippingLineId"].ToString(),
                        ShippingLineName = Result["shippingLine"].ToString(),
                        CHAId=Convert.ToInt32(Result["CHAId"]),
                        CHAName=Result["CHA"].ToString(),
                        Size = Result["Size"].ToString(),
                        Reefer = Result["Reefer"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        NoOfUnits = Result["NoOfUnits"].ToString(),
                        GrossWt = Result["GrossWt"].ToString()

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
        //public void ListOfShippingLine()
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForGateEntry", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    IList<WfldEntryThroughGateShipping> lstShippingList = new List<WfldEntryThroughGateShipping>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            lstShippingList.Add(new WfldEntryThroughGateShipping
        //            {
        //                ShippingLineId = Convert.ToInt32(Result["EximTraderId"]),
        //                ShippingLine = Convert.ToString(Result["EximTraderName"]),
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = lstShippingList;
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "No Data";
        //            _DBResponse.Data = null;
        //        }
        //    }
        //    catch (Exception ex)
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
        //public void ListOfCHA()
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetCHAForGateEntry", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    IList<WfldEntryThroughGateCHA> lstCHAList = new List<WfldEntryThroughGateCHA>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            lstCHAList.Add(new WfldEntryThroughGateCHA
        //            {
        //                CHAId = Convert.ToInt32(Result["EximTraderId"]),
        //                CHAName = Convert.ToString(Result["EximTraderName"]),
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = lstCHAList;
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "No Data";
        //            _DBResponse.Data = null;
        //        }
        //    }
        //    catch (Exception ex)
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
        public void GetAllEntryThroughGate()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = null });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<WljEntryThroughGate> LstEntryThroughGate = new List<WljEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new WljEntryThroughGate
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
            List<WljEntryThroughGate> LstEntryThroughGate = new List<WljEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new WljEntryThroughGate
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
            List<WljEntryThroughGate> LstEntryThroughGate = new List<WljEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new WljEntryThroughGate
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
            List<WljEntryThroughGate> LstEntryThroughGate = new List<WljEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new WljEntryThroughGate
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
            List<WljEntryThroughGate> LstEntryThroughGate = new List<WljEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new WljEntryThroughGate
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
            List<WljEntryThroughGate> LstEntryThroughGate = new List<WljEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new WljEntryThroughGate
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
            List<WljEntryThroughGate> LstEntryThroughGate = new List<WljEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new WljEntryThroughGate
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
            List<WljEntryThroughGate> LstEntryThroughGate = new List<WljEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new WljEntryThroughGate
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

            Wlj_ReferenceNumberCCIN ObjReferenceNumber = new Wlj_ReferenceNumberCCIN();

            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjReferenceNumber.ReferenceList.Add(new Wlj_CCINList
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
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"])
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
            WljEntryThroughGate ObjEntryThroughGate = new WljEntryThroughGate();
            string ContainerClass = "";
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
                    if (Result["ContainerType"].ToString() == "ContainerMovement" || Result["ContainerType"].ToString() == "LoadedContainerRequest")
                    {
                        ObjEntryThroughGate.ContainerType = "BacktoContainer";
                    }
                    else
                    {
                        ObjEntryThroughGate.ContainerType = Result["ContainerType"].ToString();
                    }
                    ObjEntryThroughGate.OperationType = Result["OperationType"].ToString();
                    ObjEntryThroughGate.TransportMode = Convert.ToInt32(Result["TransportMode"].ToString());
                    ObjEntryThroughGate.TransportFrom = Result["TransportFrom"].ToString();
                    ObjEntryThroughGate.ContainerLoadType = Result["ContainerLoadType"].ToString();
                    ObjEntryThroughGate.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    ObjEntryThroughGate.TPNo = Result["TPNo"].ToString();
                    ObjEntryThroughGate.IsCBT = Convert.ToInt32(Result["CBT"]) == 1 ? true : false;
                    ObjEntryThroughGate.IsOdc = Convert.ToInt32(Result["IsODC"]) == 1 ? true : false;
                    ContainerClass = Convert.ToString(Result["ContainerClass"]);
                    ObjEntryThroughGate.ExportType = Convert.ToString(Result["ExportType"]);
                    ObjEntryThroughGate.GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]);
                    ObjEntryThroughGate.GodownName = Convert.ToString(Result["GodownName"] == DBNull.Value ? "" : Result["GodownName"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjEntryThroughGate.listAddRef.Add(new Wlj_AddMoreRefForCCIN
                        {
                            addRefNo = Convert.ToString(Result["ReferenceNo"]),
                            addRefpkg = Convert.ToInt32(Result["Actual_Pkg"]),

                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjEntryThroughGate.LstSCMTR.Add(new WLJGateEntrySCMTR
                        {
                            Id = Convert.ToInt32(Result["Id"]),
                            CIMNo = Convert.ToInt32(Result["CIMNo"]),
                            CIMDate = (Result["CIMDate"] == null ? "" : Result["CIMDate"]).ToString(),
                            ReportingpartyCode = (Result["ReportingpartyCode"] == null ? "" : Result["ReportingpartyCode"]).ToString(),
                            DestinationUnlading = (Result["DestinationUnlading"] == null ? "" : Result["DestinationUnlading"]).ToString(),
                            EquipmentSize = !string.IsNullOrEmpty(Result["EquipmentSize"].ToString()) ? Result["EquipmentSize"].ToString() : "",
                            TransportMeansType = Result["TransportMeansType"].ToString(),
                            TransportMeansNo = Result["TransportMeansNo"].ToString(),
                            TotalEquipment = Convert.ToInt32(Result["TotalEquipment"]),
                            ActualArrival = (Result["ActualArrival"] == null ? "" : Result["ActualArrival"]).ToString(),
                            ContainerID = Result["ContainerID"].ToString(),
                            Equipmenttype = (Result["Equipmenttype"] == null ? "" : Result["Equipmenttype"]).ToString(),
                            EquipStatus = (Result["EquipStatus"] == null ? "" : Result["EquipStatus"]).ToString(),
                            EquipmentSerialNo = Convert.ToInt64(Result["EquipmentSerialNo"]),
                            DocumentSerialNo = Convert.ToInt64(Result["DocumentSerialNo"]),
                            DocumentTypeCode = Convert.ToString(Result["DocumentTypeCode"] == null ? "" : Result["DocumentTypeCode"]),
                            DocumentReferenceNo = Convert.ToString(Result["DocumentReferenceNo"] == null ? "" : Result["DocumentReferenceNo"]),


                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ContainerClass;
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

        #region Add vechile
        public void GetDetForExportAddVehicle(int EntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDetForExportAddVehicle", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Wlj_AddExportVehicle ObjEntryThroughGate = new Wlj_AddExportVehicle();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjEntryThroughGate.EntryId = Convert.ToInt32(Result["EntryId"]);
                    ObjEntryThroughGate.ExportReferenceNo = Result["ReferenceNo"].ToString();
                    ObjEntryThroughGate.ExportCFSCode = Result["CFSCode"].ToString();
                    ObjEntryThroughGate.TotalNoOfPkg = Convert.ToInt32(Result["NoOfPackages"]);
                    ObjEntryThroughGate.TotalGrWt = Convert.ToDecimal(Result["GrossWeight"]);


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
        public void AddEditEntryThroughGateVehicle(Wlj_AddExportVehicle ObjEntryThroughGate)
        {
            
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DtlEntryId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.DtlEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ExportCFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ExportReferenceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ExportVehicleNo });
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
                    _DBResponse.Message = "The Vehicle No Already Existed";
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
        public void GetVehicleDtlbyEntryId(int EntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetVehicleDtlByEntryId", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Wlj_AddExportVehicle> LstVehicle = new List<Wlj_AddExportVehicle>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstVehicle.Add(new Wlj_AddExportVehicle
                    {
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        DtlEntryId = Convert.ToInt32(Result["EntryDtlId"]),
                        ExportReferenceNo = Result["ReferenceNo"].ToString(),
                        ExportCFSCode = Result["CFSCode"].ToString(),
                        ExportVehicleNo = Result["VehicleNo"].ToString(),
                        ExportNoOfPkg = Convert.ToInt32(Result["NoOfPkg"]),
                        ExportGrWeight = Convert.ToDecimal(Result["GrossWeight"])


                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstVehicle;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void ListContainerClass()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            // lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            // IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainerClass", CommandType.StoredProcedure);
            IList<CwcExim.Areas.Export.Models.ContainerClass> lstContainerClass = new List<CwcExim.Areas.Export.Models.ContainerClass>();
            _DBResponse = new DatabaseResponse();
            try
            {
                // bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstContainerClass.Add(new CwcExim.Areas.Export.Models.ContainerClass
                    {
                        containerlassid = Convert.ToInt32(Result["containerlassid"]),
                        CntainerClass = Convert.ToString(Result["ContainerClas"]),
                        //PartyCode = Result["PartyCode"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstContainerClass;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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
            WljEntryThroughGate ObjEntryThroughGate = new WljEntryThroughGate();
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
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        CustomSealNo = Result["CustomSealNo"].ToString()
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
            WljEntryThroughGateBond ObjEntryThroughGate = new WljEntryThroughGateBond();
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
                    ObjEntryThroughGate.IsCBT = Convert.ToInt32(Result["CBT"]);
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

                    ObjEntryThroughGate.IsVehicle = Convert.ToInt32(Result["IsVehicle"]);
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

        public void AddEditEntryThroughGateBond(WljEntryThroughGateBond ObjEntryThroughGate)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChallanNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = "" });
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_TypeofPackage", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.TypeofPackage });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Others", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.Others });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImpName", MySqlDbType = MySqlDbType.VarChar, Value = ObjEntryThroughGate.DepositorName });
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
            WljEntryThroughGateBond objEntryThroughGate = new WljEntryThroughGateBond();
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
        public void GetAllEntryThroughGateBond()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGateBond", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<WljEntryThroughGate> LstEntryThroughGate = new List<WljEntryThroughGate>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new WljEntryThroughGate
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
    }
}