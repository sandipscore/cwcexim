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
    public class Loni_EntryThroughGateRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void AddEditEntryThroughGateCBT(PpgEntryThroughGate ObjEntryThroughGate)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportFrom", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = ObjEntryThroughGate.TransportFrom });

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


        public void AddEditEntryThroughGate(PpgEntryThroughGate ObjEntryThroughGate)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ObjEntryThroughGate.SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output });
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
                    _DBResponse.Data = ReturnObj;
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
            PpgEntryThroughGate objEntryThroughGate = new PpgEntryThroughGate();
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
            int Result = DataAccess.ExecuteNonQuery("DeleteEnrtyThroughGate", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
            PpgEntryThroughGate ObjEntryThroughGate = new PpgEntryThroughGate();
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
                    ObjEntryThroughGate.Reefer = Convert.ToBoolean(Result["Reefer"].ToString());
                    if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 1)
                    {
                        ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                    }
                    else if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 3)
                    {
                        ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                        //ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWt"]);
                        ObjEntryThroughGate.ContainerLoadType = Result["LoadType"].ToString();
                        ObjEntryThroughGate.ShippingLineSealNo = Result["SLineNo"].ToString();
                        ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    }
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
            PpgEntryThroughGate ObjEntryThroughGate = new PpgEntryThroughGate();
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
        public void ListOfShippingLine()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForGateEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PpgEntryThroughGateShipping> lstShippingList = new List<PpgEntryThroughGateShipping>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstShippingList.Add(new PpgEntryThroughGateShipping
                    {
                        ShippingLineId = Convert.ToInt32(Result["EximTraderId"]),
                        ShippingLine = Convert.ToString(Result["EximTraderName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstShippingList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCHAForGateEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PpgEntryThroughGateCHA> lstCHAList = new List<PpgEntryThroughGateCHA>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCHAList.Add(new PpgEntryThroughGateCHA
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Convert.ToString(Result["EximTraderName"]),
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
            PpgEntryThroughGate ObjEntryThroughGate = new PpgEntryThroughGate();
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
                    ObjEntryThroughGate.ContainerType = Result["ContainerType"].ToString();
                    ObjEntryThroughGate.OperationType = Result["OperationType"].ToString();
                    ObjEntryThroughGate.TransportMode = Convert.ToInt32(Result["TransportMode"].ToString());
                    ObjEntryThroughGate.TransportFrom = Result["TransportFrom"].ToString();
                    ObjEntryThroughGate.ContainerLoadType = Result["ContainerLoadType"].ToString();
                    ObjEntryThroughGate.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    ObjEntryThroughGate.TPNo = Result["TPNo"].ToString();
                    ObjEntryThroughGate.IsCBT = Convert.ToInt32(Result["CBT"]) == 1 ? true : false;
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




        #region Edit Empty Container Gate Entry Invoice 
        public void GetEmptyGateEntryInvoiceDetailsForEdit(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEmptyGateInvoiceDetailsForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            decimal hours = 0;
            Areas.Import.Models.PpgInvoiceYard objPostPaymentSheet = new Areas.Import.Models.PpgInvoiceYard();
            try
            {
                while (Result.Read())
                {
                    objPostPaymentSheet.ROAddress = Result["ROAddress"].ToString();
                    objPostPaymentSheet.CompanyName = Result["CompanyName"].ToString();
                    objPostPaymentSheet.CompanyShortName = Result["CompanyShortName"].ToString();
                    objPostPaymentSheet.CompanyAddress = Result["CompanyAddress"].ToString();
                    objPostPaymentSheet.PhoneNo = Result["PhoneNo"].ToString();
                    objPostPaymentSheet.FaxNumber = Result["FaxNumber"].ToString();
                    objPostPaymentSheet.EmailAddress = Result["EmailAddress"].ToString();
                    objPostPaymentSheet.StateId = Convert.ToInt32(Result["StateId"]);
                    objPostPaymentSheet.StateCode = Result["StateCode"].ToString();

                    objPostPaymentSheet.CityId = Convert.ToInt32(Result["CityId"]);
                    objPostPaymentSheet.GstIn = Result["GstIn"].ToString();
                    objPostPaymentSheet.Pan = Result["Pan"].ToString();

                    objPostPaymentSheet.CompGST = Result["GstIn"].ToString();
                    objPostPaymentSheet.CompPAN = Result["Pan"].ToString();
                    objPostPaymentSheet.CompStateCode = Result["StateCode"].ToString();
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPostPaymentSheet.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                        objPostPaymentSheet.InvoiceType = Convert.ToString(Result["InvoiceType"]);
                        objPostPaymentSheet.DeliveryDate = Convert.ToString(Result["DeliveryDate"]);
                        objPostPaymentSheet.InvoiceDate = Convert.ToString(Result["InvoiceDate"]);
                        objPostPaymentSheet.RequestId = Convert.ToInt32(Result["StuffingReqId"]);
                        objPostPaymentSheet.RequestNo = Convert.ToString(Result["StuffingReqNo"]);
                        objPostPaymentSheet.MonthValue = Convert.ToString(Result["MonthValue"]);
                        objPostPaymentSheet.YearValue = Convert.ToString(Result["YearValue"]);
                        objPostPaymentSheet.RequestDate = Convert.ToString(Result["StuffingReqDate"]);
                        objPostPaymentSheet.PartyId = Convert.ToInt32(Result["PartyId"]);
                        objPostPaymentSheet.PartyName = Convert.ToString(Result["PartyName"]);
                        objPostPaymentSheet.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                        objPostPaymentSheet.PayeeName = Convert.ToString(Result["PayeeName"]);
                        objPostPaymentSheet.PartyGST = Convert.ToString(Result["PartyGSTNo"]);
                        objPostPaymentSheet.PartyAddress = Convert.ToString(Result["PartyAddress"]);
                        objPostPaymentSheet.PartyState = Convert.ToString(Result["PartyState"]);
                        objPostPaymentSheet.PartyStateCode = Convert.ToString(Result["PartyStateCode"]);
                        objPostPaymentSheet.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                        objPostPaymentSheet.TotalDiscount = Convert.ToDecimal(Result["TotalDiscount"]);
                        objPostPaymentSheet.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                        objPostPaymentSheet.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                        objPostPaymentSheet.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                        objPostPaymentSheet.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                        objPostPaymentSheet.CWCTotal = Convert.ToDecimal(Result["CWCTotal"]);
                        objPostPaymentSheet.HTTotal = Convert.ToDecimal(Result["HTTotal"]);
                        objPostPaymentSheet.CWCTDSPer = Convert.ToDecimal(Result["CWCTDSPerc"]);
                        objPostPaymentSheet.HTTDSPer = Convert.ToDecimal(Result["HTTDSPerc"]);
                        objPostPaymentSheet.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                        objPostPaymentSheet.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                        objPostPaymentSheet.TDS = Convert.ToDecimal(Result["TDS"]);
                        objPostPaymentSheet.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                        objPostPaymentSheet.AllTotal = Convert.ToDecimal(Result["AllTotal"]);
                        objPostPaymentSheet.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                        objPostPaymentSheet.InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]);
                        objPostPaymentSheet.ShippingLineName = Convert.ToString(Result["ShippingLinaName"]);
                        objPostPaymentSheet.CHAName = Convert.ToString(Result["CHAName"]);
                        objPostPaymentSheet.ImporterExporter = Convert.ToString(Result["ExporterImporterName"]);
                        objPostPaymentSheet.BOENo = Convert.ToString(Result["BOENo"]);
                        objPostPaymentSheet.BOEDate = Convert.ToString(Result["BOEDate"]);
                        objPostPaymentSheet.TotalNoOfPackages = Result["TotalNoOfPackages"] == DBNull.Value ? 0 : Convert.ToInt32(Result["TotalNoOfPackages"]);
                        objPostPaymentSheet.TotalGrossWt = Result["TotalGrossWt"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalGrossWt"]);
                        objPostPaymentSheet.TotalWtPerUnit = Result["TotalWtPerUnit"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalWtPerUnit"]);
                        objPostPaymentSheet.TotalSpaceOccupied = Result["TotalSpaceOccupied"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                        objPostPaymentSheet.TotalSpaceOccupiedUnit = Result["TotalSpaceOccupiedUnit"] == DBNull.Value ? "" : Convert.ToString(Result["TotalSpaceOccupiedUnit"]);
                        objPostPaymentSheet.TotalValueOfCargo = Result["TotalValueOfCargo"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalValueOfCargo"]);
                        objPostPaymentSheet.CompGST = Convert.ToString(Result["CompGST"]);
                        objPostPaymentSheet.CompPAN = Convert.ToString(Result["CompPAN"]);
                        objPostPaymentSheet.CompStateCode = Convert.ToString(Result["CompStateCode"]);
                        objPostPaymentSheet.ApproveOn = Convert.ToString(Result["CstmExaminationDate"]);
                        objPostPaymentSheet.DestuffingDate = Convert.ToString(Result["DestuffingDate"]);
                        objPostPaymentSheet.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                        objPostPaymentSheet.CartingDate = Convert.ToString(Result["CartingDate"]);
                        objPostPaymentSheet.ArrivalDate = Convert.ToString(Result["ArrivalDate"]);
                        objPostPaymentSheet.CFSCode = Convert.ToString(Result["CFSCode"]);
                        objPostPaymentSheet.Remarks = Convert.ToString(Result["Remarks"]);
                        objPostPaymentSheet.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                        objPostPaymentSheet.Module = Convert.ToString(Result["Module"]);
                        objPostPaymentSheet.PaymentMode = Convert.ToString(Result["PaymentMode"]);
                        objPostPaymentSheet.ContainerNo = Convert.ToString(Result["Container"]);
                        objPostPaymentSheet.Size = Convert.ToString(Result["Size"]);
                        objPostPaymentSheet.FumigationType = Convert.ToString(Result["OperationType"]);
                        //  objPostPaymentSheet.ChemicalId = Convert.ToInt32(Result["ChemicalId"]);
                        objPostPaymentSheet.CFSCode = Convert.ToString(Result["CFSCode"]);
                        objPostPaymentSheet.GateInDate = Convert.ToString(Result["GateInDate"]);
                        objPostPaymentSheet.GF = Convert.ToDecimal(Result["GF"]);
                        objPostPaymentSheet.MF = Convert.ToDecimal(Result["MF"]);
                        objPostPaymentSheet.TotalSpace = Convert.ToDecimal(Result["TotalSpace"]);
                        objPostPaymentSheet.GodownName = Convert.ToString(Result["GodownName"]);

                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        if (Result["ChargeType"].ToString() == "OTI")
                        {
                            if (Convert.ToDecimal(Result["Rate"]) > 0)
                            {
                                decimal hr = Convert.ToDecimal(Result["Amount"].ToString()) / Convert.ToDecimal(Result["Rate"]);
                                hours = hr;

                                objPostPaymentSheet.OTHours = hours;
                            }
                        }
                        objPostPaymentSheet.lstPostPaymentChrg.Add(new Areas.Import.Models.PpgPostPaymentChrg()
                        {
                            ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                            Clause = Convert.ToString(Result["Clause"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            SACCode = Convert.ToString(Result["SACCode"]),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new Areas.Import.Models.PpgPostPaymentContainer()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            Reefer = Convert.ToInt16(Result["IsReefer"]),
                            Insured = Convert.ToInt16(Result["Insured"]),
                            RMS = Convert.ToInt16(Result["RMS"]),
                            CargoType = Convert.ToInt16(Result["CargoType"]),
                            //   ArrivalDate = Convert.ToString(Result["ArrivalDate"]),
                            //  DestuffingDate = Convert.ToDateTime(Result["DestuffingDate"]),
                            // CartingDate = Convert.ToDateTime(Result["CartingDate"]),
                            // StuffingDate = Convert.ToDateTime(Result["StuffingDate"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                            //   SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            //    SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"])

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstContWiseAmount.Add(new Areas.Import.Models.PpgContainerWiseAmount()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                            LineNo = Convert.ToString(Result["LineNo"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstOperationCFSCodeWiseAmount.Add(new Areas.Import.Models.PpgOperationCFSCodeWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstCharge.Add(new CwcExim.Areas.CashManagement.Models.Charge
                        {
                            ChargeId = Convert.ToInt32(Result["ChargeId"].ToString()),
                            ChargeName = Result["ChargeName"].ToString(),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPostPaymentSheet;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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



        public void GetEmptyGateInvPaymentSheetInvoice(String InvoiceDate,
          string FumigationChargeType, String InvoiceType, int PartyId, String size, String contxml, int InvoiceId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.VarChar, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = contxml });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();


            CwcExim.Areas.Export.Models.PPG_MovementInvoice objInvoice = new CwcExim.Areas.Export.Models.PPG_MovementInvoice();
            IDataReader Result = DataAccess.ExecuteDataReader("getEmptyGatepaymentsheetForEdit", CommandType.StoredProcedure, DParam);
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
                        objInvoice.lstPrePaymentCont.Add(new CwcExim.Areas.Export.Models.PpgPreInvoiceContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new CwcExim.Areas.Export.Models.PpgPostPaymentContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),

                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new CwcExim.Areas.Export.Models.PpgPostPaymentChrg
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
                    while (Result.Read())
                    {
                        objInvoice.lstContWiseAmount.Add(new CwcExim.Areas.Export.Models.PpgContainerWiseAmount
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
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new CwcExim.Areas.Export.Models.PpgOperationCFSCodeWiseAmount
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
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrgBreakup.Add(new CwcExim.Areas.Export.Models.ppgCMPostPaymentChargebreakupdate
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
                            EndDate = Result["EndDate"].ToString()
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
        public void EditEmptyGateInvoice(Areas.Export.Models.PPG_MovementInvoice ObjPostPaymentSheet,
         string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
        int BranchId, int Uid,
       string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate).ToString("yyyy-MM-dd HH:mm:ss") });
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("EditEmptyGateInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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




        public void GetPaymentPartyforEmptyGate()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyForEmptyGateInv", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<CwcExim.Areas.Import.Models.PaymentPartyName> objPaymentPartyName = new List<CwcExim.Areas.Import.Models.PaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new CwcExim.Areas.Import.Models.PaymentPartyName()
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        PartyCode = Convert.ToString(Result["PartyCode"]),
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
        #endregion
    }
}